using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebStatus.Controller
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var basePath = _configuration["PATH_BASE"];
            return Redirect($"{basePath}/hc-ui");
        }

        [HttpGet("/Config")]
        public IActionResult Config()
        {
            var configurationValues = _configuration.GetSection("HealthChecksUI:HealthChecks")
                .GetChildren()
                .SelectMany(cs => cs.GetChildren())
                .Union(_configuration.GetSection("HealthChecks-UI:HealthChecks")
                    .GetChildren()
                    .SelectMany(cs => cs.GetChildren()))
                .ToDictionary(v => v.Path, v => v.Value);

            return View(configurationValues);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
