using Microsoft.AspNetCore.Mvc;
using Runrs.DataAccess.Repository.IRepository;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;

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
            return View(clubs);
        }
    }
}
