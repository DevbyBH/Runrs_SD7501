using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Runrs.DataAccess.Repository.IRepository;
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
            // Prevent null/empty submissions
            if (user == null ||
                string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                TempData["Error"] = "Please enter username and password.";
                return RedirectToAction("Index");
            }

            var obj = _unitOfWork.User.GetByUsername(user.Username);

            if (obj != null)
            {
                var passwordHasher = new PasswordHasher<User>();

                var result = passwordHasher.VerifyHashedPassword(
                    obj,
                    obj.PasswordHash,
                    user.PasswordHash
                );

                if (result == PasswordVerificationResult.Success)
                {
                    TempData.Clear();

                    HttpContext.Session.SetInt32("UserId", obj.Id);

                    HttpContext.Session.SetString(
                        "UserRole",
                        obj.Role.ToString());

                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["Error"] = "Invalid username or password.";

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
                // Prevent duplicate usernames
                var existingUser = _unitOfWork.User.GetByUsername(user.Username);

                if (existingUser != null)
                {
                    TempData["Error"] = "Username already exists.";
                    return View(user);
                }

                // Password hashing
                var passwordHasher = new PasswordHasher<User>();

                user.PasswordHash = passwordHasher.HashPassword(
                    user,
                    user.PasswordHash);

                _unitOfWork.User.Add(user);
                _unitOfWork.User.Save();

                HttpContext.Session.SetInt32("UserId", user.Id);

                HttpContext.Session.SetString(
                    "UserRole",
                    user.Role.ToString());

                TempData["Success"] = "Welcome! Registration successful.";

                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}