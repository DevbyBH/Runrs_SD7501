using Microsoft.AspNetCore.Mvc;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;
using Microsoft.EntityFrameworkCore;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.DataAccess.Repository;

namespace Runrs_SD7501.Controllers
{
    public class ClubController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClubController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string query)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            List<Club> clubs;

            if (string.IsNullOrEmpty(query))
            {
                clubs = _unitOfWork.Club.GetAll().ToList();
            }
            else
            {
                clubs = _unitOfWork.Club.GetAll()
                    .Where(c => c.ClubName.Contains(query)
                             || c.ClubLocation.Contains(query)
                             || c.ClubDescription.Contains(query))
                    .ToList();
            }

            // get memberships for current user
            var memberships = _unitOfWork.Membership.GetAll()
                .Where(m => m.UserId == userId)
                .ToList();

            ViewBag.Memberships = memberships;

            return View(clubs);
        }
        // ----------------------- Create Club Actions ----------------------- // // 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Club club)
        {
            club.OwnerId = HttpContext.Session.GetInt32("UserId") ?? 0;
            club.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _unitOfWork.Club.Add(club);
                _unitOfWork.Save();
                TempData["Success"] = "Club created successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        // ------------------------------------------------------------------ //


        // ------------------------ Edit Club Actions ----------------------- //
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0) 
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0; 
            var club = _unitOfWork.Club.Get(c => c.Id == id); 

            if (club == null)
                return NotFound();
            if (club.OwnerId != userId) 
                return Unauthorized();

            return View(club);
        }

        [HttpPost]
        public IActionResult Edit(Club club) 
        {
            var existingClub = _unitOfWork.Club.Get(c => c.Id == club.Id); 

            if (existingClub == null)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (existingClub.OwnerId != userId) 
                return Unauthorized();

            if (ModelState.IsValid)
            { 
                existingClub.ClubName = club.ClubName;
                existingClub.ClubDescription = club.ClubDescription;
                existingClub.ClubLocation = club.ClubLocation;
                existingClub.IsPrivate = club.IsPrivate;
                existingClub.ImageUrl = club.ImageUrl;
                existingClub.Difficulty = club.Difficulty;
                existingClub.Distance = club.Distance;
                existingClub.Type = club.Type;

                _unitOfWork.Club.Update(existingClub);
                _unitOfWork.Save();
                TempData["Success"] = "Club updated successfully!";
                return RedirectToAction("Index");
            }
            return View(club);
        }
        // ------------------------------------------------------------------ //

        // ------------------------ Delete Club Actions ----------------------- // 
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _unitOfWork.Club.Get(c => c.Id == id);

            if (club == null)
                return NotFound();
            if (club.OwnerId != userId)
                return Unauthorized();

            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _unitOfWork.Club.Get(c => c.Id == id);

            if (club == null)
                return NotFound();
            _unitOfWork.Club.Remove(club);
            _unitOfWork.Save();
            TempData["Success"] = "Club deleted successfully";
            return RedirectToAction("Index"); 
        }
        // ------------------------------------------------------------------- //

        // -----------------------  Join Club Action ----------------------- //

        [HttpPost]
        public IActionResult Join(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId == 0)
                return Unauthorized();

            // check if already joined
            var existing = _unitOfWork.Membership.Get(m =>
                m.UserId == userId && m.ClubId == id);

            

            if (existing == null)
            {
                var membership = new Membership
                {
                    UserId = userId,
                    ClubId = id,
                    Role = "Member",
                    JoinedAt = DateTime.Now,
                    Status = MembershipStatus.Pending
                };

                _unitOfWork.Membership.Add(membership);
                _unitOfWork.Save();
            }



            return RedirectToAction("Details", new { id });
        }

        // -----------------------------------------------------------------//

        // -----------------------  details Club Action ----------------------- //

        public IActionResult Details(int id)
        {
            if (id == 0)
                return NotFound();

            var club = _unitOfWork.Club.Get(c => c.Id == id);

            if (club == null)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var membership = _unitOfWork.Membership.Get(m =>
                m.UserId == userId && m.ClubId == id);

            ViewBag.Membership = membership;

            return View(club);
        }

        // -------------------------------------------------------------------- //
    }
}
