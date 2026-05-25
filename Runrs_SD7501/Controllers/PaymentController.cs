using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models;
using Stripe;
using Stripe.Checkout;

namespace Runrs_SD7501.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly StripeSettings _stripeSettings;

        public PaymentController(IUnitOfWork unitOfWork, IOptions<StripeSettings> stripeSettings)
        {
            _unitOfWork = unitOfWork;
            _stripeSettings = stripeSettings.Value;
        }

        public IActionResult Checkout(int eventId)
        {
            var runEvent = _unitOfWork.Event.Get(e => e.Id == eventId, includeProperties: "Club");
            if (runEvent == null) return NotFound();

            ViewBag.PublishableKey = _stripeSettings.PublishableKey;
            return View(runEvent);
        }

        [HttpPost]
        public IActionResult CreateCheckoutSession(int eventId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var runEvent = _unitOfWork.Event.Get(e => e.Id == eventId, includeProperties: "Club");

            if (runEvent == null) return NotFound();

            var domain = $"{Request.Scheme}://{Request.Host}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "nzd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = runEvent.EventTitle,
                                Description = $"{runEvent.Club?.ClubName} — {runEvent.EventDate:dd MMM yyyy}"
                            },
                            UnitAmount = (long)(runEvent.EntryFee * 100)
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{domain}/Payment/Success?eventId={eventId}&userId={userId}",
                CancelUrl = $"{domain}/Club/Details/{runEvent.ClubId}"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

        public IActionResult Success(int eventId, int userId)
        {
            var runEvent = _unitOfWork.Event.Get(e => e.Id == eventId);
            if (runEvent == null) return NotFound();

            var existing = _unitOfWork.EventRegistration.Get(r =>
                r.UserId == userId && r.EventId == eventId);

            if (existing == null)
            {
                var registration = new EventRegistration
                {
                    UserId = userId,
                    EventId = eventId,
                    RegisteredAt = DateTime.Now,
                    Status = RegistrationStatus.Confirmed,
                    PaymentStatus = PaymentStatus.Paid
                };

                _unitOfWork.EventRegistration.Add(registration);
                _unitOfWork.Save();
            }

            TempData["Success"] = "Payment successful! You are now registered for the event.";
            return RedirectToAction("Details", "Club", new { id = runEvent.ClubId });
        }

        public IActionResult Cancel(int eventId)
        {
            var runEvent = _unitOfWork.Event.Get(e => e.Id == eventId);
            TempData["Error"] = "Payment was cancelled.";
            return RedirectToAction("Details", "Club", new { id = runEvent?.ClubId });
        }

        public IActionResult CartCheckout()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var cartItems = _unitOfWork.ShoppingCart
                .GetAll(includeProperties: "Event,Event.Club")
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            var domain = $"{Request.Scheme}://{Request.Host}";

            var lineItems = cartItems.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "nzd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Event.EventTitle,
                        Description = $"{item.Event.Club?.ClubName} — {item.Event.EventDate:dd MMM yyyy}"
                    },
                    UnitAmount = (long)(item.Event.EntryFee * 100)
                },
                Quantity = item.Count
            }).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = $"{domain}/Payment/CartSuccess?userId={userId}",
                CancelUrl = $"{domain}/ShoppingCart/Index"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

        public IActionResult CartSuccess(int userId)
        {
            var cartItems = _unitOfWork.ShoppingCart
                .GetAll(includeProperties: "Event")
                .Where(c => c.UserId == userId)
                .ToList();

            foreach (var item in cartItems)
            {
                var existing = _unitOfWork.EventRegistration.Get(r =>
                    r.UserId == userId && r.EventId == item.EventId);

                if (existing == null)
                {
                    var registration = new EventRegistration
                    {
                        UserId = userId,
                        EventId = item.EventId,
                        RegisteredAt = DateTime.Now,
                        Status = RegistrationStatus.Confirmed,
                        PaymentStatus = PaymentStatus.Paid
                    };
                    _unitOfWork.EventRegistration.Add(registration);
                }

                _unitOfWork.ShoppingCart.Remove(item);
            }

            _unitOfWork.Save();
            TempData["Success"] = "Payment successful! You are now registered for all events.";
            return RedirectToAction("Index", "Club");
        }

        public IActionResult CartCancel()
        {
            TempData["Error"] = "Payment was cancelled.";
            return RedirectToAction("Index", "ShoppingCart");
        }

    }
}