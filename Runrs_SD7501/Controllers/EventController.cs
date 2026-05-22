using Microsoft.AspNetCore.Mvc;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models;
using System;
using System.IO;

namespace Runrs_SD7501.Controllers
{
    public class EventController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EventController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
            var registrations = _unitOfWork.EventRegistration.GetAll(includeProperties: "User")
                .Where(r => r.EventId == id && r.Status == RegistrationStatus.Confirmed)
                .ToList();

            ViewBag.Registration = registration;
            ViewBag.Registrations = registrations;
            ViewBag.UserId = userId;
            ViewBag.AvailableSpots = runEvent.MaxParticipants - registrations.Count;

            return PartialView("_EventDetailsPartial", runEvent); // ← return partial view
        }

        [HttpPost]
        public async Task<IActionResult> Create(RunEvent runEvent, IFormFile? imageFile)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var club = _unitOfWork.Club.Get(c => c.Id == runEvent.ClubId);

            if (club == null) return NotFound();
            if (club.OwnerId != userId) return Unauthorized();
            runEvent.CreatedAt = DateTime.Now;
            ModelState.Remove("ImageUrl");
            if (runEvent.EntryFee > 0 && runEvent.EntryFee < 0.01m)
            {
                ModelState.AddModelError("EntryFee", "Please enter a valid amount for paid events");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("imageFile", "Invalid file type. Allowed: JPG, PNG, GIF");
                            ViewBag.ClubId = runEvent.ClubId;
                            return View(runEvent);
                        }
                        if (imageFile.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("imageFile", "File size must be less than 5MB");
                            ViewBag.ClubId = runEvent.ClubId;
                            return View(runEvent);
                        }
                        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "event-images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                        runEvent.ImageUrl = $"/uploads/event-images/{uniqueFileName}";
                    }
                    _unitOfWork.Event.Add(runEvent);
                    _unitOfWork.Save();
                    TempData["Success"] = "Event created successfully!";
                    return RedirectToAction("Details", "Club", new { id = runEvent.ClubId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error uploading file: {ex.Message}");
                }
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
        public async Task<IActionResult> Edit(RunEvent runEvent, IFormFile? imageFile)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var existingEvent = _unitOfWork.Event.Get(e => e.Id == runEvent.Id, includeProperties: "Club");

            if (existingEvent == null) return NotFound();
            if (existingEvent.Club.OwnerId != userId) return Unauthorized();
            ModelState.Remove("ImageUrl");
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("imageFile", "Invalid file type. Allowed: JPG, PNG, GIF");
                            return View(runEvent);
                        }
                        if (imageFile.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("imageFile", "File size must be less than 5MB");
                            return View(runEvent);
                        }
                        if (!string.IsNullOrEmpty(existingEvent.ImageUrl))
                        {
                            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingEvent.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "event-images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                        runEvent.ImageUrl = $"/uploads/event-images/{uniqueFileName}";
                    }
                    else
                    {
                        runEvent.ImageUrl = existingEvent.ImageUrl;
                    }

                    existingEvent.EventTitle = runEvent.EventTitle;
                    existingEvent.EventDescription = runEvent.EventDescription;
                    existingEvent.EventDate = runEvent.EventDate;
                    existingEvent.EventLocation = runEvent.EventLocation;
                    existingEvent.Distance = runEvent.Distance;
                    existingEvent.MaxParticipants = runEvent.MaxParticipants;
                    existingEvent.EntryFee = runEvent.EntryFee;
                    existingEvent.ImageUrl = runEvent.ImageUrl;

                    _unitOfWork.Event.Update(existingEvent);
                    _unitOfWork.Save();
                    TempData["Success"] = "Event updated successfully!";
                    return RedirectToAction("Details", "Club", new { id = existingEvent.ClubId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error uploading file: {ex.Message}");
                }
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
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var runEvent = _unitOfWork.Event.Get(e => e.Id == id, includeProperties: "Club");
            if (runEvent == null) return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (runEvent.Club.OwnerId != userId) return Unauthorized();
            var registrations = _unitOfWork.EventRegistration.GetAll().Where(r => r.EventId == runEvent.Id).ToList();
            _unitOfWork.EventRegistration.RemoveRange(registrations);

            _unitOfWork.Event.Remove(runEvent);
            _unitOfWork.Save();

            TempData["Success"] = "Event deleted successfully!";
            return RedirectToAction("Details", "Club", new { id = runEvent.ClubId });
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
                return RedirectToAction("Details", "Club", new { id = runEvent.ClubId });
            }
            var currentCount = _unitOfWork.EventRegistration.GetAll().Count(r => r.EventId == eventId && r.Status == RegistrationStatus.Confirmed);
            if (currentCount >= runEvent.MaxParticipants)
            {
                TempData["Error"] = "There are no more available spots!";
                return RedirectToAction("Details", "Club", new { id = runEvent.ClubId });
            }
            if (runEvent.EntryFee == null || runEvent.EntryFee == 0)
            {
                var registration = new EventRegistration
                {
                    UserId = userId, EventId = eventId, RegisteredAt = DateTime.Now, Status = RegistrationStatus.Confirmed, PaymentStatus = PaymentStatus.Unpaid
                };

                _unitOfWork.EventRegistration.Add(registration);
                _unitOfWork.Save();
                TempData["Success"] = "You have successfully registered for the event!";
                return RedirectToAction("Details", "Club", new { id = runEvent.ClubId });
            }
            HttpContext.Session.SetInt32("PendingEventId", eventId);
            return RedirectToAction("Checkout", "Payment", new { eventId });
        }

        [HttpPost]
        public IActionResult CancelRegistration(int eventId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var registration = _unitOfWork.EventRegistration.Get(r => r.UserId == userId && r.EventId == eventId);
            if (registration == null) return NotFound();
            var runEvent = _unitOfWork.Event.Get(e => e.Id == eventId);
            _unitOfWork.EventRegistration.Remove(registration);
            _unitOfWork.Save();
            TempData["Success"] = "You have unregistered from this event.";

            return RedirectToAction("Details", "Club", new { id = runEvent?.ClubId });
        }
    }
}
