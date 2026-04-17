using Microsoft.AspNetCore.Mvc;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;

namespace Runrs_SD7501.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user != null)
            {
                var obj = _db.Users.FirstOrDefault(a => a.Username == user.Username && a.PasswordHash == user.PasswordHash); //<-- Byron 17/04/2026 - Edited to allow for proper session login (So clubs can be created with correct user id)

                if (obj != null) //<-- Byron 17/04/2026 - Edited to allow for proper session login (So clubs can be created with correct user id)
                {
                    HttpContext.Session.SetInt32("UserId", obj.Id);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }

            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();

                // auto login
                HttpContext.Session.SetInt32("UserId", user.Id); //<-- Byron 17/04/2026 - Edited to allow for proper session login (So clubs can be created with correct user id)
                TempData["Success"] = "Welcome! Registration successful.";
                return RedirectToAction("Index", "Home"); ;
            }

            return View();
        }
    }
}
