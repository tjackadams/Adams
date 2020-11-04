using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Adams.Services.Identity.Api.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}