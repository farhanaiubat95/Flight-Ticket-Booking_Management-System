using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using Main_Part.Data;
using Main_Part.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Main_Part.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Booking/BookNow/5
        public IActionResult BookNow(int id)
        {
            var tour = _context.Tours_table.Find(id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // POST: Booking/BookNow
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookNow(int tourId, int passengerCount)
        {
            var tour = await _context.Tours_table.FindAsync(tourId);
            if (tour == null)
            {
                return NotFound();
            }
            if (tour.AvailableSeats + passengerCount > tour.Maxperson)
            {
                ModelState.AddModelError("", "Cannot exceed the maximum number of people for this tour.");
                return View(tour);
            }

            tour.Maxperson -= passengerCount;
            tour.AvailableSeats = 0 + tour.AvailableSeats + passengerCount;
            _context.Tours_table.Update(tour);
            await _context.SaveChangesAsync();

            // // Check if the booking exceeds the max allowed number of passengers
            // var totalBookedSeats = _context.Bookings.Where(b => b.TourId == tourId).Sum(b => b.PassengerCount);


            var user = await _userManager.GetUserAsync(User);
            var booking = new Booking
            {
                TourId = tourId,
                UserId = user?.Id,
                PassengerCount = passengerCount,
                TotalAmount = passengerCount * tour.Price + 10,
                Status = "Booked",
                BookingDate = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("BookingList");
        }

        // GET: Booking/BookingList
        public async Task<IActionResult> BookingList()
        {
            var userId = (await _userManager.GetUserAsync(User))?.Id;
            var bookings = await _context.Bookings.Where(b => b.UserId == userId).Include(b => b.Tour).ToListAsync();
            return View(bookings);

        }
        public IActionResult CancelBooking(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null && (DateTime.Now - booking.BookingDate).TotalHours <= 24)
            {
                booking.Status = "Cancelled";
                booking.CanceledDate = DateTime.Now;
                _context.Bookings.Update(booking);
                _context.SaveChanges();
            }
            return RedirectToAction("BookingList");
        }

        public IActionResult GenerateTicket(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                var ticket = GeneratePDF(booking);
                return File(ticket, "application/pdf", "Ticket.pdf");
            }
            return NotFound();
        }
        private byte[] GeneratePDF(Booking booking)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Create a new PDF document
                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add a title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                document.Add(new Paragraph("Flight Ticket", titleFont));
                document.Add(new Paragraph("\n"));

                // Add ticket details
                document.Add(new Paragraph($"Ticket Number: {booking.TicketNumber}", regularFont));
                document.Add(new Paragraph($"Booking ID: {booking.BookingId}", regularFont));
                document.Add(new Paragraph($"User ID: {booking.UserId}", regularFont));
                document.Add(new Paragraph($"Booking Date: {booking.BookingDate:dd/MM/yyyy}", regularFont));
                document.Add(new Paragraph($"Status: {booking.Status}", regularFont));
                document.Add(new Paragraph("\n"));

                document.Add(new Paragraph("Thank you for booking with us!", regularFont));

                // Close the document
                document.Close();

                // Return the PDF as a byte array
                return memoryStream.ToArray();
            }
        }
    }

}