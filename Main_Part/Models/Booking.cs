using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string? PaymentID { get; set; }
        public int TourId { get; set; }
        public string? UserId { get; set; }
        public string? BookUserNsme { get; set; }
        public int PassengerCount { get; set; }
        [Precision(16, 2)]
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; } // "Booked", "Pending", "Canceled"
        public DateTime BookingDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? CancelDeadline { get; set; }
        public string? TicketNumber { get; set; }
        public string? PaymentOption { get; set; }
        public string? PaymentStatus { get; set; } // "Paid" or "Pending"
        public string? TransactionId { get; set; } // Unique transaction ID for payments
        public DateTime? PaymentDate { get; set; }

        // Navigation Properties

        public Tours? Tour { get; set; }
        public ApplicationUser? User { get; set; }
    }


}