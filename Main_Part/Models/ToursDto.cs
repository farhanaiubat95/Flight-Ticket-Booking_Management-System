using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Main_Part.Models
{
    public class ToursDto
    {
        [Required, MaxLength(100)]
        public string FlightFrom { get; set; } = "";

        [Required, MaxLength(100)]
        public string FlightTo { get; set; } = "";
        [Required]
        public DateTime DepartureDate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime ArrivalDate { get; set; } = DateTime.UtcNow;
        [Required]
        public decimal Price { get; set; } = 0;
        [Required]
        public int Maxperson { get; set; } = 0;
        // when we create a product, we need to upload a photo
        // when we update a product, PhotoFile is optional , so ? sign is used
        public IFormFile? PhotoFile { get; set; }
    }
}