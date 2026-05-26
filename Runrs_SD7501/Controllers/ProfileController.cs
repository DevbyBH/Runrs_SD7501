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
    }
}