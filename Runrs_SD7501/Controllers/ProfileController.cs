using Microsoft.AspNetCore.Mvc;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models.ViewModels;
using Runrs.Models;
using Microsoft.AspNetCore.Hosting;

namespace Runrs_SD7501.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Details(int id)
        {
            var user = _unitOfWork.User.Get(u => u.Id == id);

            if (user == null)
                return NotFound();

            int currentUserId =
                HttpContext.Session.GetInt32("UserId") ?? 0;

            var friendship = _unitOfWork.Friendship.Get(
                f =>
                    (f.RequesterId == currentUserId &&
                     f.AddresseeId == id) ||

                    (f.RequesterId == id &&
                     f.AddresseeId == currentUserId),

                includeProperties: "Requester,Addressee"
            );

            var friends = _unitOfWork.Friendship
                .GetAll(includeProperties: "Requester,Addressee")
                .Where(f =>
                    (f.RequesterId == id ||
                     f.AddresseeId == id) &&

                    f.Status == FriendshipStatus.Accepted)
                .ToList();

            var activities = new List<ActivityItem>();

            var vm = new ProfileVM
            {
                User = user,
                CurrentUserId = currentUserId,
                Friendship = friendship,
                Friends = friends,
                Activities = activities
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddFriend(int id)
        {
            int userId =
                HttpContext.Session.GetInt32("UserId") ?? 0;

            // Prevent self-friending
            if (userId == id)
            {
                return RedirectToAction(
                    "Details",
                    new { id });
            }

            var existing = _unitOfWork.Friendship.Get(f =>
                (f.RequesterId == userId &&
                 f.AddresseeId == id) ||

                (f.RequesterId == id &&
                 f.AddresseeId == userId));

            if (existing == null)
            {
                var friendship = new Friendship
                {
                    RequesterId = userId,
                    AddresseeId = id,
                    CreatedAt = DateTime.Now,
                    Status = FriendshipStatus.Pending
                };

                _unitOfWork.Friendship.Add(friendship);
                _unitOfWork.Save();

                TempData["Success"] = "Friend request sent!";
            }

            return RedirectToAction(
                "Details",
                new { id });
        }

        [HttpPost]
        public IActionResult AcceptFriend(int friendshipId)
        {
            var friendship =
                _unitOfWork.Friendship.Get(
                    f => f.Id == friendshipId);

            if (friendship == null)
                return NotFound();

            friendship.Status = FriendshipStatus.Accepted;

            _unitOfWork.Friendship.Update(friendship);
            _unitOfWork.Save();

            return RedirectToAction(
                "Details",
                new { id = friendship.RequesterId });
        }

        [HttpPost]
        public IActionResult RemoveFriend(int id)
        {
            int userId =
                HttpContext.Session.GetInt32("UserId") ?? 0;

            var friendship = _unitOfWork.Friendship.Get(f =>
                (f.RequesterId == userId &&
                 f.AddresseeId == id) ||

                (f.RequesterId == id &&
                 f.AddresseeId == userId));

            if (friendship == null)
                return NotFound();

            _unitOfWork.Friendship.Remove(friendship);
            _unitOfWork.Save();

            return RedirectToAction(
                "Details",
                new { id });
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(int id, IFormFile? profileImage)
        {
            int currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (currentUserId != id)
                return Unauthorized();

            var user = _unitOfWork.User.Get(u => u.Id == id);
            if (user == null)
                return NotFound();

            if (profileImage != null && profileImage.Length > 0)
            {
                try
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["Error"] = "Invalid file type. Allowed: JPG, PNG, GIF, WEBP";
                        return RedirectToAction("Details", new { id });
                    }
                    if (profileImage.Length > 5 * 1024 * 1024)
                    {
                        TempData["Error"] = "File size must be less than 5MB";
                        return RedirectToAction("Details", new { id });
                    }
                    if (!string.IsNullOrEmpty(user.ProfileImageUrl) &&
                        !user.ProfileImageUrl.Contains("default-avatar"))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                            user.ProfileImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profile-pictures");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(fileStream);
                    }

                    user.ProfileImageUrl = $"/uploads/profile-pictures/{uniqueFileName}";

                    _unitOfWork.User.Update(user);
                    _unitOfWork.Save();

                    TempData["Success"] = "Profile picture updated successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error uploading image: {ex.Message}";
                }
            }

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public IActionResult RemoveProfilePicture(int id)
        {
            int currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (currentUserId != id)
                return Unauthorized();

            var user = _unitOfWork.User.Get(u => u.Id == id);
            if (user == null)
                return NotFound();
            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                    user.ProfileImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            user.ProfileImageUrl = null;
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
            TempData["Success"] = "Profile picture removed successfully!";
            return RedirectToAction("Details", new { id });
        }
    }
}