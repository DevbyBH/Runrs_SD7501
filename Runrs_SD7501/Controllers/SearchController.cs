using Microsoft.AspNetCore.Mvc;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models;

namespace Runrs_SD7501.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // ----------------------- Search Club Page Actions ----------------------- 
        public IActionResult Index(string query)
        {
            List<Club> clubs;

            if (string.IsNullOrEmpty(query))
            {
                clubs = _unitOfWork.Club.GetAll(includeProperties: "Owner").ToList(); // <-- Shows ALL clubs on the Search Page
            }
            else
            {
                clubs = _unitOfWork.Club.GetAll(includeProperties: "Owner").Where(c => c.ClubName.Contains(query) || c.ClubLocation.Contains(query) || c.ClubDescription.Contains(query)).ToList();
            }

            var memberCounts = _unitOfWork.Membership.GetAll().Where(m => m.Status == MembershipStatus.Approved).GroupBy(m => m.ClubId).ToDictionary(g => g.Key, g => g.Count());
            ViewBag.MemberCounts = memberCounts;
            return View(clubs);
        }
    }
}
