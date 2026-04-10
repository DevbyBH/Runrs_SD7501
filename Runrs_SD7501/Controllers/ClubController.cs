using Microsoft.AspNetCore.Mvc;

namespace Runrs_SD7501.Controllers
{
    public class ClubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
