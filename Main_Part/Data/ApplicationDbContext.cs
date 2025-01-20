using Main_Part.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Main_Part.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tours> Tours_table { get; set; } // Tours = Models.Tours.cs , Tours_table = Table name
    public DbSet<Booking> Bookings { get; set; }// Booking = Models.Booking.cs , Bookings = Table name

}