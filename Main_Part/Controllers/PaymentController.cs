using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Main_Part.Data;
using Main_Part.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Main_Part.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> PaymentOptions(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound();

            var tour = _context.Tours_table.FirstOrDefault(t => t.Id == booking.TourId);
            if (tour == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            ViewData["FlightFrom"] = tour.FlightFrom;
            ViewData["FlightTo"] = tour.FlightTo;
            ViewData["fullname"] = booking.BookUserNsme; // <== This will now appear
            ViewData["userEmail"] = await _userManager.GetEmailAsync(user);
            ViewData["userPhoneNumber"] = await _userManager.GetPhoneNumberAsync(user);

            return View(booking);
        }


        [HttpPost]
        public IActionResult PayNow(int bookingId, string paymentOption)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound();

            string todayDate = DateTime.UtcNow.ToString("yyyyMMdd");

            var lastPayment = _context.Bookings
                                .Where(b => b.PaymentID != null && b.PaymentID.StartsWith($"PAY{todayDate}"))
                                .OrderByDescending(b => b.PaymentID)
                                .FirstOrDefault();

            int nextSerial = 1; // First serial
            if (lastPayment != null)
            {
                //Correct slicing: "PAY20250507-0001" → take substring after position 11 (index 11 = after '-')
                string lastSerialStr = lastPayment.PaymentID!.Substring(11);
                if (int.TryParse(lastSerialStr, out int lastSerial))
                {
                    nextSerial = lastSerial + 1;
                }
            }

            string newPaymentID = $"PAY{todayDate}-{nextSerial.ToString("D4")}";

            booking.PaymentID = newPaymentID;
            booking.PaymentOption = paymentOption;
            booking.PaymentStatus = "Paid";
            booking.Status = "Booked";
            booking.TicketNumber = Guid.NewGuid().ToString();
            booking.TransactionId = Guid.NewGuid().ToString();
            booking.PaymentDate = DateTime.UtcNow;

            _context.SaveChanges();

            return RedirectToAction("PaymentStatus", new { bookingId = booking.BookingId });
        }

        public async Task<IActionResult> PaymentStatus(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var userId = user.Id;

            ViewData["fullname"] = booking.BookUserNsme;
            ViewData["userEmail"] = await _userManager.GetEmailAsync(user);
            ViewData["userPhoneNumber"] = await _userManager.GetPhoneNumberAsync(user);
            
            var tour = _context.Tours_table.FirstOrDefault(t => t.Id == booking.TourId);
            if (tour == null) return NotFound();
            ViewData["FlightFrom"] = tour.FlightFrom;
            ViewData["FlightTo"] = tour.FlightTo;

            return View(booking);
        }
    }
}