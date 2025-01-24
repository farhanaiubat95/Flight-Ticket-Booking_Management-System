using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Main_Part.Data;
using Main_Part.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Main_Part.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult PaymentOptions(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound();

            var tour = _context.Tours_table.FirstOrDefault(t => t.Id == booking.TourId);
            if (tour == null) return NotFound();
            ViewData["FlightFrom"] = tour.FlightFrom;
            ViewData["FlightTo"] = tour.FlightTo;
            ViewData["fullname"] =booking.BookUserNsme;
            return View(booking);
        }

        [HttpPost]
        public IActionResult PayNow(int bookingId, string paymentOption)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound();

            booking.PaymentOption = paymentOption;
            booking.PaymentStatus = "Paid";
            booking.Status = "Booked";
            booking.TicketNumber = Guid.NewGuid().ToString();
            booking.TransactionId = Guid.NewGuid().ToString();

            _context.SaveChanges();

            // Send confirmation email (pseudo-code)
            // EmailService.SendBookingConfirmation(booking);

            return RedirectToAction("PaymentStatus", new { bookingId = booking.BookingId });
        }

        public IActionResult PaymentStatus(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound();

            return View(booking);
        }
    }
}