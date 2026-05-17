using Microsoft.AspNetCore.Mvc;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models;

namespace Runrs_SD7501.Controllers
{
    public class EventController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int clubId)
        {
            return RedirectToAction("Details", "Club", new { id = clubId });
        }

        public IActionResult Create(int clubId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _unitOfWork.Club.Get(c => c.Id == clubId);

            if (club == null) return NotFound();
            if (club.OwnerId != userId) return Unauthorized();

            ViewBag.ClubId = clubId;
            ViewBag.ClubName = club.ClubName;
            return View();
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            var runEvent = _unitOfWork.Event.Get(e => e.Id == id, includeProperties: "Club");
            if (runEvent == null) return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var registration = _unitOfWork.EventRegistration.Get(r => r.EventId == id && r.UserId == userId);
            var registrations = _unitOfWork.EventRegistration.GetAll(includeProperties: "User").Where(r => r.EventId == id && r.Status == RegistrationStatus.Confirmed).ToList();

            ViewBag.Registration = registration;
            ViewBag.Registrations = registrations;
            ViewBag.UserId = userId;
            ViewBag.AvailableSpots = runEvent.MaxParticipants - registrations.Count;

            return View(runEvent);
        }

        [HttpPost]
        public IActionResult Create(RunEvent runEvent)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _unitOfWork.Club.Get(c => c.Id == runEvent.ClubId);

            if (club == null) return NotFound();
            if (club.OwnerId != userId) return Unauthorized();
            runEvent.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _unitOfWork.Event.Add(runEvent);
                _unitOfWork.Save();
                TempData["Success"] = "Event created successfully!";
                return RedirectToAction("Details", "Club", new { clubId = runEvent.ClubId });
            }
            ViewBag.ClubId = runEvent.ClubId;
            return View(runEvent);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var runEvent = _unitOfWork.Event.Get(e => e.Id == id, includeProperties: "Club");

            if (runEvent == null) return NotFound();
            if (runEvent.Club.OwnerId != userId) return Unauthorized();

            return View(runEvent);
        }

        [HttpPost]
        public IActionResult Edit(RunEvent runEvent)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var existingEvent = _unitOfWork.Event.Get(e => e.Id == runEvent.Id, includeProperties: "Club");

            if (existingEvent == null) return NotFound();
            if (existingEvent.Club.OwnerId != userId) return Unauthorized();

            if (ModelState.IsValid)
            {
                existingEvent.EventTitle = runEvent.EventTitle;
                existingEvent.EventDescription = runEvent.EventDescription;
                existingEvent.EventDate = runEvent.EventDate;
                existingEvent.EventLocation = runEvent.EventLocation;
                existingEvent.Distance = runEvent.Distance;
                existingEvent.MaxParticipants = runEvent.MaxParticipants;
                existingEvent.EntryFee = runEvent.EntryFee;

                _unitOfWork.Event.Update(existingEvent);
                _unitOfWork.Save();
                TempData["Success"] = "Event updated successfully!";
                return RedirectToAction("Details", "Club", new { clubId = existingEvent.ClubId });
            }
            return View(runEvent);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var runEvent = _unitOfWork.Event.Get(e => e.Id == id, includeProperties: "Club");

            if (runEvent == null) return NotFound();
            if (runEvent.Club.OwnerId != userId) return Unauthorized();

            return View(runEvent);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var runEvent = _unitOfWork.Event.Get(e => e.Id == id, includeProperties: "Club");
            if (runEvent == null) return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (runEvent.Club.OwnerId != userId) return Unauthorized();

            _unitOfWork.Event.Remove(runEvent);
            _unitOfWork.Save();
            TempData["Success"] = "Event deleted successfully!";
            return RedirectToAction("Details", "Club", new { clubId = runEvent.ClubId });
        }

        [HttpPost]
        public IActionResult Register(int eventId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var runEvent = _unitOfWork.Event.Get(e => e.Id == eventId, includeProperties: "Club");

            if (runEvent == null) return NotFound();

            var existingRegistration = _unitOfWork.EventRegistration.Get(r => r.EventId == eventId && r.UserId == userId);
            if (existingRegistration != null)
            {
                TempData["Error"] = "You are already registered for this event.";
                return RedirectToAction("Details", new { id = eventId });
            }

            var currentCount = _unitOfWork.EventRegistration.GetAll().Count(r => r.EventId == eventId && r.Status == RegistrationStatus.Confirmed);
            if (currentCount >= runEvent.MaxParticipants)
            {
                TempData["Error"] = "There are no more available spots!.";
                return RedirectToAction("Details", new { id = eventId });
            }

            if (runEvent.EntryFee == 0) // <---- Byron (16/05/2026) - For free events, automatically confirm registration without payment
            {
                var registration = new EventRegistration
                {
                    UserId = userId,
                    EventId = eventId,
                    RegisteredAt = DateTime.Now,
                    Status = RegistrationStatus.Confirmed,
                    PaymentStatus = PaymentStatus.Unpaid
                };

                _unitOfWork.EventRegistration.Add(registration);
                _unitOfWork.Save();
                TempData["Success"] = "You have successfully registered for the event!";
                return RedirectToAction("Details", new { id = eventId });

            }
            HttpContext.Session.SetInt32("PendingEventId", eventId); // Byron (16/05/2026) - Store the event ID in session for payment processing!
            return RedirectToAction("Checkout", "Payment", new { eventId });

        }

        [HttpPost]
        public IActionResult CancelRegistration(int eventId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var registration = _unitOfWork.EventRegistration.Get(r => r.UserId == userId && r.EventId == eventId);

            if (registration == null) return NotFound();

            _unitOfWork.EventRegistration.Remove(registration);
            _unitOfWork.Save();
            TempData["Success"] = "You have unregistered from this event.";
            return RedirectToAction("Details", new { id = eventId });
        }
    }
}
