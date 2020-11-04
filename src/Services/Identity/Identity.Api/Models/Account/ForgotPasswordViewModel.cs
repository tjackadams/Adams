using System.ComponentModel.DataAnnotations;

namespace Adams.Services.Identity.Api.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
