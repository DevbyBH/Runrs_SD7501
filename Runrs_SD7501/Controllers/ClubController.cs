using Microsoft.AspNetCore.Mvc;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;
using Microsoft.EntityFrameworkCore;

namespace Runrs_SD7501.Controllers
{
    public class ClubController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public ClubController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() // <------ Byron 17/04/2026 - Had to change due to reconfiguration of the SearchController & Search Views
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var clubs = _context.Clubs.Include(c => c.Owner).Where(c => c.OwnerId == userId).ToList();
            return View(clubs);
        }
        // ----------------------- Create Club Actions ----------------------- // // <------ Byron 10/04/2026
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
                _context.Clubs.Add(club);
                _context.SaveChanges();
                TempData["Success"] = "Club created successfully!";
                return RedirectToAction("Index"); // <------ Byron 17/04/2026 - Had to change due to reconfiguration of the SearchController & Search Views
            }
            return View();
        }
        // ------------------------------------------------------------------ //


        // ------------------------ Edit Club Actions ----------------------- // <------ Byron 18/04/2026 - Modified GET to allow for secure editing/viewing by authorized club owner (So clubs can be edited with correct user id) -- Copied across to Delete Action
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0) // <-- Byron 18/04/2026 - Check if the id is null or 0 (invalid). If invalid return not found.
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0; //<-- Byron 18/04/2026 Get the logged in user's ID from the session
            var club = _context.Clubs.FirstOrDefault(c => c.Id == id); //<-- Byron 18/04/2026 - Find the club from the database based on the provided id

            if (club == null)
                return NotFound();
            if (club.OwnerId != userId) //<-- Byron 18/04/2026 - Check if the logged in user is the owner of the club. If not the owner, return unauthorized to prevent access to edit/viewing of the club
                return Unauthorized();

            return View(club);
        }

        [HttpPost]
        public IActionResult Edit(Club club) //<-- Byron 18/04/2026 - Modified POST to allow for secure editing/viewing by authorized club owner (So clubs can be edited with correct user id) -- Copied across to Delete Action
        {
            var existingClub = _context.Clubs.FirstOrDefault(c => c.Id == club.Id); //<-- Byron 18/04/2026 - Get the existing club from the database

            if (existingClub == null)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0; //<-- Byron 18/04/2026 Get the logged in user's ID from the session
            if (existingClub.OwnerId != userId) //<-- Byron 18/04/2026 - Check if the logged in user is the owner of the club
                return Unauthorized();
            if (ModelState.IsValid)
            {
                // Byron 18/04/2026 - Update the existing club's properties based on edits (Keep the same owenerID)
                existingClub.ClubName = club.ClubName;
                existingClub.ClubDescription = club.ClubDescription;
                existingClub.ClubLocation = club.ClubLocation;
                existingClub.IsPrivate = club.IsPrivate;
                existingClub.ImageUrl = club.ImageUrl;
                existingClub.Difficulty = club.Difficulty;
                existingClub.Distance = club.Distance;
                existingClub.Type = club.Type;

                _context.SaveChanges();
                TempData["Success"] = "Club updated successfully!";
                return RedirectToAction("Index");
            }

            return View(club);
        }
        // ------------------------------------------------------------------ //

        // ------------------------ Delete Club Actions ----------------------- // <------ Byron 10/04/2026
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _context.Clubs.FirstOrDefault(c => c.Id == id);

            if (club == null)
                return NotFound();

            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _context.Clubs.FirstOrDefault(c => c.Id == id);

            if (club == null)
                return NotFound();
            _context.Clubs.Remove(club);
            _context.SaveChanges();
            TempData["Success"] = "Club deleted successfully";
            return RedirectToAction("Index"); // <------ Byron 17/04/2026 - Had to change due to reconfiguration of the SearchController & Search Views
        }
        // ------------------------------------------------------------------- //
    }
}
