using Microsoft.AspNetCore.Mvc;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.DataAccess.Data;
using Runrs.Models;

namespace Runrs_SD7501.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                var obj = _unitOfWork.User.GetByUsername(user.Username); 

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
                _unitOfWork.User.Add(user);
                _unitOfWork.User.Save();
                HttpContext.Session.SetInt32("UserId", user.Id);

                HttpContext.Session.SetString(
                    "UserRole",                         
                    user.Role.ToString());

                TempData["Success"] = "Welcome! Registration successful.";
                return RedirectToAction("Index", "Home"); ;
            }
            return View();
        }
    }
}
