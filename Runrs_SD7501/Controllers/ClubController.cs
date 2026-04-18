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
        private readonly IClubRepository _clubRepository;
        public ClubController(IClubRepository db)
        {
            _clubRepository = db;
        }

        public IActionResult Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var clubs = _clubRepository.GetAll(includeProperties: "Owner")
                .Where(c => c.OwnerId == userId)
                .ToList();
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
                _clubRepository.Add(club);
                _clubRepository.Save();
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
            var club = _clubRepository.Get(c => c.Id == id); 

            if (club == null)
                return NotFound();
            if (club.OwnerId != userId) 
                return Unauthorized();

            return View(club);
        }

        [HttpPost]
        public IActionResult Edit(Club club) 
        {
            var existingClub = _clubRepository.Get(c => c.Id == club.Id); 

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

                _clubRepository.Update(existingClub);
                _clubRepository.Save();
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
            var club = _clubRepository.Get(c => c.Id == id);

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
            var club = _clubRepository.Get(c => c.Id == id);

            if (club == null)
                return NotFound();
            _clubRepository.Remove(club);
            _clubRepository.Save();
            TempData["Success"] = "Club deleted successfully";
            return RedirectToAction("Index"); 
        }
        // ------------------------------------------------------------------- //
    }
}
