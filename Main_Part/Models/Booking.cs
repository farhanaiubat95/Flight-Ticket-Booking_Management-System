using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main_Part.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Main_Part.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int TourId { get; set; }
        public string? UserId { get; set; }
        public int PassengerCount { get; set; }
        [Precision(16, 2)]
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; } // "Booked", "Pending", "Canceled"
        public DateTime BookingDate { get; set; }
        public DateTime? CanceledDate { get; set; }

        // Navigation Properties
        public Tours? Tour { get; set; }
        public ApplicationUser? User { get; set; }
    }


}