using Microsoft.AspNetCore.Mvc;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;

namespace Runrs_SD7501.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        // ----------------------- Search Club Page Actions ----------------------- //<------ Byron 17/04/2026 - New SearchController & Search Views to allow users to search for clubs by name, location, or description. Also shows all clubs if no search query is entered.
        public IActionResult Index(string query)
        {
            List<Club> clubs;

            if (string.IsNullOrEmpty(query))
            {
                clubs = _context.Clubs.ToList(); // <-- Shows ALL clubs on the Search Page
            }
            else
            {
                clubs = _context.Clubs.Where(c => c.ClubName.Contains(query) || c.ClubLocation.Contains(query) || c.ClubDescription.Contains(query)).ToList();
            }
            return View(clubs);
        }
    }
}
