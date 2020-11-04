using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Adams.Services.Identity.Api.Models
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }
    }
}
