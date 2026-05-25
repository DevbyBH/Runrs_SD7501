using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Runrs.Models;
using Runrs.Models.ViewModels;
using Runrs.DataAccess.Repository.IRepository;

namespace Runrs_SD7501.Controllers;

public class HomeController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return View();
        }

        // ================= MY MEMBERSHIPS =================

        var myMemberships = _unitOfWork.Membership
            .GetAll(includeProperties: "Club")
            .Where(m =>
                m.UserId == userId.Value &&
                m.Status == MembershipStatus.Approved)
            .ToList();

        ViewBag.MyClubs = myMemberships
            .Select(m => m.Club)
            .Where(c => c != null)
            .ToList();

        ViewBag.ClubCount = myMemberships.Count;

        // ================= MY CLUB IDS =================

        var myClubIds = myMemberships
            .Select(m => m.ClubId)
            .ToList();

        // ================= UPCOMING EVENTS =================

        ViewBag.UpcomingEvents = _unitOfWork.Event
            .GetAll(includeProperties: "Club")
            .Where(e =>
                myClubIds.Contains(e.ClubId) &&
                e.EventDate > DateTime.Now)
            .OrderBy(e => e.EventDate)
            .Take(5)
            .ToList();

        // ================= THIS WEEK RUNS =================

        var weekStart = DateTime.Now.Date;
        var weekEnd = weekStart.AddDays(7);

        ViewBag.ThisWeekRuns = _unitOfWork.Event
            .GetAll()
            .Count(e =>
                myClubIds.Contains(e.ClubId) &&
                e.EventDate >= weekStart &&
                e.EventDate <= weekEnd);

        // ================= SUGGESTED CLUBS =================

        ViewBag.SuggestedClubs = _unitOfWork.Club
            .GetAll()
            .Where(c =>
                !myClubIds.Contains(c.Id) &&
                !c.IsPrivate)
            .OrderByDescending(c => c.CreatedAt)
            .Take(3)
            .ToList();

        // ================= RECENT ACTIVITY =================

        var activities = new List<ActivityItem>();

        // ================= FRIENDSHIP ACTIVITIES =================

        var friendships = _unitOfWork.Friendship
            .GetAll(includeProperties: "Requester,Addressee")
            .Where(f =>
                f.RequesterId == userId.Value ||
                f.AddresseeId == userId.Value)
            .ToList();

        foreach (var f in friendships)
        {
            // ================= PENDING =================

            if (f.Status == FriendshipStatus.Pending)
            {
                // Sent request
                if (f.RequesterId == userId.Value)
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
                if (f.AddresseeId == userId.Value)
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
                var friendName = f.RequesterId == userId.Value
                    ? f.Addressee?.Username
                    : f.Requester?.Username;

                var friendId = f.RequesterId == userId.Value
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

        // ================= CLUB MEMBERSHIP ACTIVITIES =================

        foreach (var membership in myMemberships)
        {
            // Pending
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

            // Approved
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

        // ================= SORT ACTIVITIES =================

        activities = activities
            .OrderByDescending(a => a.Date)
            .Take(10)
            .ToList();

        ViewBag.RecentActivity = activities;

        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId =
                Activity.Current?.Id ??
                HttpContext.TraceIdentifier
        });
    }
}