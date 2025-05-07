using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Main_Part.Data;
using Main_Part.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Main_Part.Controllers
{
    [Authorize]
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TourController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        // GET: Tour/List
        [HttpGet("list")]
        public IActionResult List(string? searchTerm)
        {
            var tours = _context.Tours_table.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tours = tours.Where(t =>
                    t.FlightFrom.ToLower().Contains(searchTerm.ToLower()) ||
                    t.FlightTo.ToLower().Contains(searchTerm.ToLower()) ||
                    t.DepartureDate.ToString().Contains(searchTerm) ||
                    t.ArrivalDate.ToString().Contains(searchTerm) ||
                    t.Price.ToString().Contains(searchTerm) 
                );
            }

            return View(tours.ToList());
        }

        // GET: Tour/BookNow/5
        [HttpGet("booknow/{id}")]
        public IActionResult BookNow(int id)
        {
            var tour = _context.Tours_table.Find(id);
            if (tour == null) return NotFound(); // Ensure tour exists
            return View(tour);
        }


    }

}