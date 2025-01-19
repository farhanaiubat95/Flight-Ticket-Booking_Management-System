
using Microsoft.AspNetCore.Identity;

namespace Main_Part.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Names { get; set; }

        public string? ProfilePicture { get; set; }
    }
}