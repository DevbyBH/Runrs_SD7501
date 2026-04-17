using Microsoft.AspNetCore.Mvc;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;

namespace Runrs_SD7501.Controllers
{
    public class ClubController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public ClubController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult MyClubs()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            List<Club> clubs = _context.Clubs.Where(c => c.OwnerId == userId).ToList();
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
            if (ModelState.IsValid)
            {
                _context.Clubs.Add(club);
                _context.SaveChanges();
                TempData["Success"] = "Club created successfully!";
                return RedirectToAction("MyClubs");
            }
            return View();
        }
        // ------------------------------------------------------------------ //


        // ------------------------ Edit Club Actions ----------------------- // <------ Byron 10/04/2026
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Club? club = _context.Clubs.Find(id);
            if (club == null)
                return NotFound();

            return View(club);
        }

        [HttpPost]
        public IActionResult Edit(Club club)
        {
            if (ModelState.IsValid)
            {
                _context.Clubs.Update(club);
                _context.SaveChanges();
                TempData["Success"] = "Club updated successfully!";
                return RedirectToAction("MyClubs");
            }
            return View();
        }
        // ------------------------------------------------------------------ //

        // ------------------------ Delete Club Actions ----------------------- // <------ Byron 10/04/2026
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Club? club = _context.Clubs.Find(id);

            if (club == null)
                return NotFound();

            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Club? club = _context.Clubs.Find(id);

            if (club == null)
                return NotFound();

            _context.Clubs.Remove(club);
            _context.SaveChanges();
            TempData["Success"] = "Club deleted successfully";
            return RedirectToAction("MyClubs");
        }
        // ------------------------------------------------------------------- //

        // ----------------------- Search Club Page Actions ----------------------- // // <------ Byron 17/04/2026
        public IActionResult Search(string query)
        {
            List<Club> clubs;

            if (string.IsNullOrEmpty(query))
            {
                clubs = _context.Clubs.ToList(); // ← all clubs
            }
            else
            {
                clubs = _context.Clubs
                    .Where(c => c.ClubName.Contains(query) || c.ClubLocation.Contains(query) ||c.ClubDescription.Contains(query)).ToList();
            }

            return View(clubs);
        }
    }
}
