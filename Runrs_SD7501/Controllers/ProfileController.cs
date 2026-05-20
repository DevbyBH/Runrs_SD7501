using Microsoft.AspNetCore.Mvc;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models.ViewModels;
using Runrs.Models;

namespace Runrs_SD7501.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Details(int id)
        {
            var user = _unitOfWork.User.Get(u => u.Id == id);

            if (user == null)
                return NotFound();

            int currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            // ================= CURRENT FRIENDSHIP =================

            var friendship = _unitOfWork.Friendship.Get(
                f =>
                    (f.RequesterId == currentUserId && f.AddresseeId == id) ||
                    (f.RequesterId == id && f.AddresseeId == currentUserId),
                includeProperties: "Requester,Addressee"
            );

            // ================= FRIENDS =================

            var friends = _unitOfWork.Friendship
                .GetAll(includeProperties: "Requester,Addressee")
                .Where(f =>
                    (f.RequesterId == id || f.AddresseeId == id) &&
                    f.Status == FriendshipStatus.Accepted)
                .ToList();

            // ================= ACTIVITIES =================

            var activities = new List<ActivityItem>();

            // ================= FRIENDSHIP ACTIVITIES =================

            var friendships = _unitOfWork.Friendship
                .GetAll(includeProperties: "Requester,Addressee")
                .Where(f => f.RequesterId == id || f.AddresseeId == id)
                .ToList();

            foreach (var f in friendships)
            {
                // ================= PENDING =================

                if (f.Status == FriendshipStatus.Pending)
                {
                    // Sent request
                    if (f.RequesterId == id)
                    {
                        activities.Add(new ActivityItem
                        {
                            Message =
                                $"You sent a friend request to {f.Addressee?.Username}",

                            Date = f.CreatedAt,

                            Type = "warning",

                            Url = Url.Action(
                                "Details",
                                "Profile",
                                new { id = f.AddresseeId })
                        });
                    }

                    // Received request
                    if (f.AddresseeId == id)
                    {
                        activities.Add(new ActivityItem
                        {
                            Message =
                                $"{f.Requester?.Username} sent you a friend request",

                            Date = f.CreatedAt,

                            Type = "warning",

                            Url = Url.Action(
                                "Details",
                                "Profile",
                                new { id = f.RequesterId })
                        });
                    }
                }

                // ================= ACCEPTED =================

                if (f.Status == FriendshipStatus.Accepted)
                {
                    var friendName = f.RequesterId == id
                        ? f.Addressee?.Username
                        : f.Requester?.Username;

                    var friendId = f.RequesterId == id
                        ? f.AddresseeId
                        : f.RequesterId;

                    activities.Add(new ActivityItem
                    {
                        Message =
                            $"You became friends with {friendName}",

                        Date = f.CreatedAt,

                        Type = "success",

                        Url = Url.Action(
                            "Details",
                            "Profile",
                            new { id = friendId })
                    });
                }
            }

            // ================= CLUB MEMBERSHIPS =================

            var memberships = _unitOfWork.Membership
                .GetAll(includeProperties: "Club")
                .Where(m => m.UserId == id)
                .ToList();

            foreach (var membership in memberships)
            {
                // Pending Request
                if (membership.Status == MembershipStatus.Pending)
                {
                    activities.Add(new ActivityItem
                    {
                        Message =
                            $"Requested to join '{membership.Club?.ClubName}'",

                        Date = membership.JoinedAt,

                        Type = "warning",

                        Url = Url.Action(
                            "Details",
                            "Club",
                            new { id = membership.ClubId })
                    });
                }

                // Approved Membership
                if (membership.Status == MembershipStatus.Approved)
                {
                    activities.Add(new ActivityItem
                    {
                        Message =
                            $"Joined club '{membership.Club?.ClubName}'",

                        Date = membership.JoinedAt,

                        Type = "info",

                        Url = Url.Action(
                            "Details",
                            "Club",
                            new { id = membership.ClubId })
                    });

                    activities.Add(new ActivityItem
                    {
                        Message =
                            $"Membership approved for '{membership.Club?.ClubName}'",

                        Date = membership.JoinedAt,

                        Type = "success",

                        Url = Url.Action(
                            "Details",
                            "Club",
                            new { id = membership.ClubId })
                    });
                }
            }

            // ================= SORT =================

            activities = activities
                .OrderByDescending(a => a.Date)
                .ToList();

            // ================= VIEWMODEL =================

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
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var existing = _unitOfWork.Friendship.Get(f =>
                (f.RequesterId == userId && f.AddresseeId == id) ||
                (f.RequesterId == id && f.AddresseeId == userId));

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

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public IActionResult AcceptFriend(int friendshipId)
        {
            var friendship = _unitOfWork.Friendship.Get(f => f.Id == friendshipId);

            if (friendship == null) return NotFound();

            friendship.Status = FriendshipStatus.Accepted;

            _unitOfWork.Friendship.Update(friendship);
            _unitOfWork.Save();

            return RedirectToAction("Details", new { id = friendship.RequesterId });
        }

        [HttpPost]
        public IActionResult RemoveFriend(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var friendship = _unitOfWork.Friendship.Get(f =>
                (f.RequesterId == userId && f.AddresseeId == id) ||
                (f.RequesterId == id && f.AddresseeId == userId));

            if (friendship == null) return NotFound();

            _unitOfWork.Friendship.Remove(friendship);
            _unitOfWork.Save();

            return RedirectToAction("Details", new { id });
        }

        //--------------------------------------------------------------------------------------------

        // ================= Bio ACTIONS =================

        [HttpPost]
        public IActionResult UpdateBio(int id, string bio)
        {
            int currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (currentUserId != id)
                return Unauthorized();

            var user = _unitOfWork.User.Get(u => u.Id == id);

            if (user == null)
                return NotFound();

            user.Bio = bio;

            _unitOfWork.User.Update(user);
            _unitOfWork.Save();

            TempData["Success"] = "Bio updated successfully!";

            return RedirectToAction("Details", new { id });
        }
    }
}
