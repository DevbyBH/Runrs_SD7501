using Microsoft.AspNetCore.Mvc;
using Runrs.Models;
using Microsoft.EntityFrameworkCore;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.DataAccess.Repository;

namespace Runrs_SD7501.Controllers
{
    public class ClubController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClubController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string query)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var ownedClubs = _unitOfWork.Club.GetAll(includeProperties: "Owner").Where(c => c.OwnerId == userId).ToList(); // Byron 22/04//2026 - Shows clubs owned by the logged-in user on the My Clubs Page
            var joinedClubIds = _unitOfWork.Membership.GetAll().Where(m => m.UserId == userId && m.Status == MembershipStatus.Approved).Select(m => m.ClubId).ToList(); // Byron 22/04//2026 - Gets the IDs of clubs the user has joined
            var joinedClubs = _unitOfWork.Club.GetAll(includeProperties: "Owner").Where(c => joinedClubIds.Contains(c.Id) && c.OwnerId != userId).ToList(); // Byron 22/04//2026 - Shows clubs the user has "joined" on the My Clubs Page
            ViewBag.OwnedClubs = ownedClubs;
            ViewBag.JoinedClubs = joinedClubs;
            return View(ownedClubs);
        }
        // ----------------------- Create Club Actions ----------------------- // // 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Club club)
        {
            club.OwnerId = HttpContext.Session.GetInt32("UserId") ?? 0;
            club.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _unitOfWork.Club.Add(club);
                _unitOfWork.Save();
                TempData["Success"] = "Club created successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        // ------------------------------------------------------------------ //


        // ------------------------ Edit Club Actions ----------------------- //
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _unitOfWork.Club.Get(c => c.Id == id);

            if (club == null)
                return NotFound();
            if (club.OwnerId != userId)
                return Unauthorized();

            return View(club);
        }

        [HttpPost]
        public IActionResult Edit(Club club)
        {
            var existingClub = _unitOfWork.Club.Get(c => c.Id == club.Id);

            if (existingClub == null)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (existingClub.OwnerId != userId)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                existingClub.ClubName = club.ClubName;
                existingClub.ClubDescription = club.ClubDescription;
                existingClub.ClubLocation = club.ClubLocation;
                existingClub.IsPrivate = club.IsPrivate;
                existingClub.ImageUrl = club.ImageUrl;
                existingClub.Difficulty = club.Difficulty;
                existingClub.Distance = club.Distance;
                existingClub.Type = club.Type;

                _unitOfWork.Club.Update(existingClub);
                _unitOfWork.Save();
                TempData["Success"] = "Club updated successfully!";
                return RedirectToAction("Index");
            }
            return View(club);
        }
        // ------------------------------------------------------------------ //

        // ------------------------ Delete Club Actions ----------------------- // 
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _unitOfWork.Club.Get(c => c.Id == id);

            if (club == null)
                return NotFound();
            if (club.OwnerId != userId)
                return Unauthorized();

            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _unitOfWork.Club.Get(c => c.Id == id);

            if (club == null)
                return NotFound();
            _unitOfWork.Club.Remove(club);
            _unitOfWork.Save();
            TempData["Success"] = "Club deleted successfully";
            return RedirectToAction("Index");
        }
        // ------------------------------------------------------------------- //

        // -----------------------  Join Club Action ----------------------- //

        [HttpPost]
        public IActionResult Join(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId == 0)
                return Unauthorized();

            // check if already joined
            var existing = _unitOfWork.Membership.Get(m =>
                m.UserId == userId && m.ClubId == id);



            if (existing == null)
            {
                var club = _unitOfWork.Club.Get(c => c.Id == id);
                var membership = new Membership
                {
                    UserId = userId,
                    ClubId = id,
                    Role = "Member",
                    JoinedAt = DateTime.Now,
                    Status = club != null && club.IsPrivate ? MembershipStatus.Pending : MembershipStatus.Approved
                };

                _unitOfWork.Membership.Add(membership);
                _unitOfWork.Save();
                TempData["Success"] = club != null && club.IsPrivate ? "Request to join has been sent!" : "Joined club successfully!";
            }
            else
            {
                TempData["Error"] = "You have already joined this club.";
            }
            return RedirectToAction("Details", new { id });
        }

        // -----------------------------------------------------------------//

        // -----------------------  Details Club Action ----------------------- //

        public IActionResult Details(int id)
        {
            if (id == 0)
                return NotFound();

            var club = _unitOfWork.Club.Get(c => c.Id == id, includeProperties: "Owner");

            if (club == null)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var membership = _unitOfWork.Membership.Get(m =>
                m.UserId == userId && m.ClubId == id);

            ViewBag.Membership = membership;
            ViewBag.UserId = userId;

            var members = _unitOfWork.Membership
                .GetAll(includeProperties: "User")
                .Where(m => m.ClubId == id && m.Status == MembershipStatus.Approved)
                .ToList();

            ViewBag.Members = members;

            return View(club);
        }

        // -------------------------------------------------------------------- //

        // -----------------------  Leave Club Action ----------------------- //
        [HttpPost]
        public IActionResult Leave(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var membership = _unitOfWork.Membership.Get(m => m.UserId == userId && m.ClubId == id);
            if (membership == null)
                return NotFound();
            _unitOfWork.Membership.Remove(membership);
            _unitOfWork.Save();
            TempData["Success"] = "You have left this club!";
            return RedirectToAction("Details", new { id });
        }
        // ----------------------------------------------------------------- //

        // ----------------------- Manage Membership Requests ----------------------- //

        public IActionResult ManageRequests(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var club = _unitOfWork.Club.Get(c => c.Id == id);

            if (club == null)
                return NotFound();

            if (club.OwnerId != userId)
                return Unauthorized();

            var pendingRequests = _unitOfWork.Membership
                .GetAll(includeProperties: "User")
                .Where(m => m.ClubId == id && m.Status == MembershipStatus.Pending)
                .ToList();

            ViewBag.Club = club;

            return View(pendingRequests);
        }

        [HttpPost]
        public IActionResult ApproveRequest(int membershipId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var membership = _unitOfWork.Membership.Get(
                m => m.Id == membershipId,
                includeProperties: "Club"
            );

            if (membership == null)
                return NotFound();

            if (membership.Club.OwnerId != userId)
                return Unauthorized();

            membership.Status = MembershipStatus.Approved;

            _unitOfWork.Membership.Update(membership);
            _unitOfWork.Save();

            TempData["Success"] = "Membership request approved!";

            return RedirectToAction("ManageRequests", new { id = membership.ClubId });
        }

        [HttpPost]
        public IActionResult RejectRequest(int membershipId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var membership = _unitOfWork.Membership.Get(
                m => m.Id == membershipId,
                includeProperties: "Club"
            );

            if (membership == null)
                return NotFound();

            if (membership.Club.OwnerId != userId)
                return Unauthorized();

            membership.Status = MembershipStatus.Rejected;

            _unitOfWork.Membership.Update(membership);
            _unitOfWork.Save();

            TempData["Success"] = "Membership request rejected!";

            return RedirectToAction("ManageRequests", new { id = membership.ClubId });
        }


    }
}
