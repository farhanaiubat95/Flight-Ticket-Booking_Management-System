using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Main_Part.Models
{
    public class Tours
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string FlightFrom { get; set; } = "";

        [MaxLength(100)]
        public string FlightTo { get; set; } = "";
        public DateTime DepartureDate { get; set; } = DateTime.Now;
        public DateTime ArrivalDate { get; set; } = DateTime.Now;

        [Precision(16, 2)]
        public decimal Price { get; set; } = 0;
        [MaxLength(100)]
        public int Maxperson { get; set; } = 0;

        public int AvailableSeats { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
        [MaxLength(100)]
        public string Photo { get; set; } = "";
        public DateTime CreateAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
   
}