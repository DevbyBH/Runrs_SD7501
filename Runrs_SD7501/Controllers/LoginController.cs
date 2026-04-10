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
                var obj = _db.Users.Where(a => a.Username.Equals(user.Username) && a.PasswordHash.Equals(user.PasswordHash));

                if (obj.Count<User>() == 1)
                {
                    HttpContext.Session.SetString("Id", user.Username);
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
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }
}
