using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Runrs.Models;
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

        // Dashboard logic for new home page (Dashboard) <-- Byron (18/05/2026)

        var myMemberships = _unitOfWork.Membership
            .GetAll(includeProperties: "Club")
            .Where(m => m.UserId == userId.Value && m.Status == MembershipStatus.Approved)
            .ToList();

        ViewBag.MyClubs = myMemberships
            .Select(m => m.Club)
            .Where(c => c != null)
            .ToList();

        ViewBag.ClubCount = myMemberships.Count;

        var myClubIds = myMemberships.Select(m => m.ClubId).ToList();

        ViewBag.UpcomingEvents = _unitOfWork.Event
            .GetAll(includeProperties: "Club")
            .Where(e => myClubIds.Contains(e.ClubId) && e.EventDate > DateTime.Now)
            .OrderBy(e => e.EventDate)
            .Take(5)
            .ToList();

        var weekStart = DateTime.Now.Date;
        var weekEnd = weekStart.AddDays(7);
        ViewBag.ThisWeekRuns = _unitOfWork.Event
            .GetAll()
            .Count(e => myClubIds.Contains(e.ClubId) &&
                         e.EventDate >= weekStart &&
                         e.EventDate <= weekEnd);

        ViewBag.SuggestedClubs = _unitOfWork.Club
            .GetAll()
            .Where(c => !myClubIds.Contains(c.Id) && !c.IsPrivate)
            .OrderByDescending(c => c.CreatedAt)
            .Take(3)
            .ToList();

        ViewBag.RecentActivity = myMemberships
            .OrderByDescending(m => m.JoinedAt)
            .Take(3)
            .ToList();

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}