using Microsoft.AspNetCore.Mvc;
using Runrs.DataAccess.Repository.IRepository;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;

namespace Runrs_SD7501.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        public LoginController(IUserRepository db)
        {
            _userRepository = db;
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
                var obj = _userRepository.GetByUsername(user.Username); 

                if (obj != null && obj.PasswordHash == user.PasswordHash) 
                {
                    HttpContext.Session.SetInt32("UserId", obj.Id);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    TempData["Error"] = "Invalid username or password.";
                    return RedirectToAction("Index", "Login");
                }
            }
            return RedirectToAction("Index", "Login");
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
                _userRepository.Add(user);
                _userRepository.Save();

                // auto login
                HttpContext.Session.SetInt32("UserId", user.Id);
                TempData["Success"] = "Welcome! Registration successful.";
                return RedirectToAction("Index", "Home"); ;
            }

            return View();
        }
    }
}
