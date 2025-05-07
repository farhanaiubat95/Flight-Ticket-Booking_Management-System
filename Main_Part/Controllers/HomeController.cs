using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Main_Part.Models;
using Main_Part.Data;
using Microsoft.AspNetCore.Authorization;

namespace Main_Part.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly IWebHostEnvironment environment;

    public HomeController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        this.context = context;
        this.environment = environment;
    }


    public IActionResult Index()
    {
        var tours = context.Tours_table.ToList();
        return View(tours);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult Search(string flightFrom, string flightTo)
    {
        var query = context.Tours_table.AsQueryable();

        if (!string.IsNullOrEmpty(flightFrom))
        {
            query = query.Where(t => t.FlightFrom.ToLower().Contains(flightFrom.ToLower()));
        }

        if (!string.IsNullOrEmpty(flightTo))
        {
            query = query.Where(t => t.FlightTo.ToLower().Contains(flightTo.ToLower()));
        }

        var searchResults = query.ToList();

        return View("Index", searchResults); // Show result in Home/Index.cshtml
    }

}
