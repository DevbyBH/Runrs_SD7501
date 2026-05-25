using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models;

namespace Runrs_SD7501.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var cartItems = _unitOfWork.ShoppingCart.GetAll(includeProperties:"Event,Event.Club").Where(c => c.UserId == userId).ToList();
            var total = cartItems.Sum( c => c.Event.EntryFee * c.Count);
            ViewBag.Total = total;
            return View(cartItems);
        }
        [HttpPost]
        public IActionResult AddToCart (int eventId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var runEvent = _unitOfWork.Event.Get(e => e.Id == eventId);
            if (runEvent == null) return NotFound();

            var existing = _unitOfWork.ShoppingCart.Get(c => c.UserId == userId && c.EventId == eventId);
            if (existing != null)
            {
                TempData["Error"] = "Event Ticket Already in your Cart!";
                return RedirectToAction("Details", "Club", new { id = runEvent.ClubId });
            }

            var registered = _unitOfWork.EventRegistration.Get(r => r.UserId == userId && r.EventId == eventId);
            if (registered != null)
            {
                TempData["Error"] = "You are already registered for this event!";
                return RedirectToAction("Details", "Club", new { id = runEvent.ClubId });
            }

            var cartItem = new ShoppingCart
            {
                UserId = userId,
                EventId = eventId,
                Count = 1
            };

            _unitOfWork.ShoppingCart.Add(cartItem);
            _unitOfWork.Save();
            TempData["Success"] = "Event Ticket Added to Cart!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int cartId)
        {
            var cartItem = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            if (cartItem == null) return NotFound();

            _unitOfWork.ShoppingCart.Remove(cartItem);
            _unitOfWork.Save();
            TempData["Success"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var cartItems = _unitOfWork.ShoppingCart.GetAll(includeProperties: "Event").Where(c => c.UserId == userId).ToList();
            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("CartCheckout", "true");
            return RedirectToAction("CartCheckout", "Payment");
        }
    }
}
