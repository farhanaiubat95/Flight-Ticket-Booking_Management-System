using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main_Part.Data;
using Main_Part.Models;
using Microsoft.EntityFrameworkCore;

namespace Main_Part.Services
{
    public class TourService : ITourService
    {
        private readonly ApplicationDbContext _context;

        public TourService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tours>> GetAvailableToursAsync()
        {
            return await _context.Tours_table
                .Where(t => t.AvailableSeats > 0)
                .Select(t => new Tours { /* map properties from Tours to Tour */ })
                .ToListAsync();
        }
        public async Task<bool> BookTourAsync(int tourId, string userId)
        {
            var tour = await _context.Tours_table.FindAsync(tourId);

            if (tour == null || tour.AvailableSeats <= 0)
                return false;

            // Update the available seats
            tour.AvailableSeats--;

            var booking = new Booking
            {
                TourId = tourId,
                UserId = userId,
                BookingDate = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}