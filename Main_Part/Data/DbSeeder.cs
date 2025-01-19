using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main_Part.Constants;
using Microsoft.AspNetCore.Identity;

namespace Main_Part.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            //Seed Roles
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            if (userManager == null)
            {
                throw new InvalidOperationException("User manager is not registered in the service provider.");
            }
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            if (roleManager != null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }
            else
            {
                // Handle the case where roleManager is null
                throw new InvalidOperationException("Role manager is not registered in the service provider.");
            }

            // creating admin

            var user = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Names = "Farhana Bente Islam",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }

    }
}