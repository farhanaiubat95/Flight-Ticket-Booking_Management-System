using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Main_Part.Data;
using Main_Part.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Main_Part.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public InventoryController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        // search
        public IActionResult GetAll(string? searchTerm)
        {
            var tours = context.Tours_table.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tours = tours.Where(t =>
                    t.FlightFrom.ToLower().Contains(searchTerm.ToLower()) ||
                    t.FlightTo.ToLower().Contains(searchTerm.ToLower()) ||
                    t.DepartureDate.ToString().Contains(searchTerm) ||
                    t.ArrivalDate.ToString().Contains(searchTerm)
                );
            }

            tours = tours.OrderByDescending(t => t.Id);
            return View(tours.ToList());
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(ToursDto tourDto)
        {
            // Check for null or invalid photo file
            if (tourDto.PhotoFile == null)
            {
                ModelState.AddModelError("PhotoFile", "Please select a photo.");
            }

            if (!ModelState.IsValid)
            {
                return View(tourDto);
            }

            //save image file to wwwroot/tourAddImage
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(tourDto.PhotoFile!.FileName);

            string photoFullPath = environment.WebRootPath + "/tourAddImage/" + newFileName;
            using (var stream = System.IO.File.Create(photoFullPath))
            {
                tourDto.PhotoFile.CopyTo(stream);
            }
            // Set minimum valid date for SQL datetime
            DateTime createAtDate = DateTime.Now < new DateTime(1753, 1, 1) ? new DateTime(1753, 1, 1) : DateTime.Now;

            // Map DTO to Entity and save to database
            Tours tour = new Tours
            {
                FlightFrom = tourDto.FlightFrom,
                FlightTo = tourDto.FlightTo,
                DepartureDate = tourDto.DepartureDate,
                ArrivalDate = tourDto.ArrivalDate,
                Price = tourDto.Price,
                Maxperson = tourDto.Maxperson,
                Photo = newFileName,
                CreateAt = createAtDate
            };

            context.Tours_table.Add(tour);
            context.SaveChanges();

            return RedirectToAction("GetAll", "Inventory");

        }
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var tour = context.Tours_table.Find(id);
            if (tour == null)
            {
                return RedirectToAction("GetAll", "Inventory");
            }

            var tourDto = new ToursDto()
            {
                FlightFrom = tour.FlightFrom,
                FlightTo = tour.FlightTo,
                DepartureDate = tour.DepartureDate,
                ArrivalDate = tour.ArrivalDate,
                Price = tour.Price,
                Maxperson = tour.Maxperson,
            };

            ViewData["ToursId"] = tour.Id;
            ViewData["ToursImage"] = tour.Photo;
            ViewData["ToursCreateAt"] = tour.CreateAt.ToString("MM/dd/yyyy HH:mm:ss");
            ViewData["ToursUpdatedAt"] = tour.UpdatedAt.ToString("MM/dd/yyyy HH:mm:ss");
            return View(tourDto);

        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, ToursDto tourDto)
        {
            var tour = context.Tours_table.Find(id);
            if (tour == null)
            {
                return RedirectToAction("GetAll", "Inventory");
            }

            if (!ModelState.IsValid)
            {

                ViewData["ToursId"] = tour.Id;
                ViewData["ToursImage"] = tour.Photo;
                ViewData["ToursCreateAt"] = tour.CreateAt.ToString("MM/dd/yyyy HH:mm:ss");
                ViewData["ToursUpdatedAt"] = tour.UpdatedAt.ToString("MM/dd/yyyy HH:mm:ss");
                return View(tourDto);
            }

            //UpdatedAt image file to wwwroot/tourAddImage
            string newFileName = tour.Photo;

            if (tourDto.PhotoFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(tourDto.PhotoFile!.FileName);

                string photoFullPath = environment.WebRootPath + "/tourAddImage/" + newFileName;
                using (var stream = System.IO.File.Create(photoFullPath))
                {
                    tourDto.PhotoFile.CopyTo(stream);
                }

                //delete old image
                string oldFullPath = environment.WebRootPath + "/tourAddImage/" + tour.Photo;
                System.IO.File.Delete(oldFullPath);

            }

            //UpdatedAt record to database
            tour.FlightFrom = tourDto.FlightFrom;
            tour.FlightTo = tourDto.FlightTo;
            tour.DepartureDate = tourDto.DepartureDate;
            tour.ArrivalDate = tourDto.ArrivalDate;
            tour.Price = tourDto.Price;
            tour.Maxperson = tourDto.Maxperson;
            tour.Photo = newFileName;
            tour.UpdatedAt = DateTime.Now;
            context.SaveChanges();
            return RedirectToAction("GetAll", "Inventory");


        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var tour = context.Tours_table.Find(id);
            if (tour == null)
            {
                return RedirectToAction("GetAll", "Inventory");
            }

            //delete old image
            string photoFullPath = environment.WebRootPath + "/tourAddImage/" + tour.Photo;
            System.IO.File.Delete(photoFullPath);

            context.Tours_table.Remove(tour);
            context.SaveChanges(true);

            return RedirectToAction("GetAll", "Inventory");
        }

        // View the tour information
        [HttpGet("view/{id}")]
        public IActionResult View(int id)
        {
            var tour = context.Tours_table.Find(id);
            if (tour == null)
            {
                return RedirectToAction("GetAll", "Inventory");
            }

            ViewData["ToursId"] = tour.Id;
            ViewData["FlightFrom"] = tour.FlightFrom;
            ViewData["FlightTo"] = tour.FlightTo;
            ViewData["DepartureDate"] = tour.DepartureDate;
            ViewData["ArrivalDate"] = tour.ArrivalDate;
            ViewData["Price"] = tour.Price;
            ViewData["Maxperson"] = tour.Maxperson.ToString();
            ViewData["ToursImage"] = tour.Photo;
            ViewData["ToursCreateAt"] = tour.CreateAt.ToString("MM/dd/yyyy HH:mm:ss");
            return View();

        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminBookings()
        {
            var bookings = context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.User)
                .ToList();
            return View(bookings);
        }

    }
}