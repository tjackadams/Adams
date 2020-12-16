using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ApiGw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration((hostingContext, configuration) =>
                    {
                        configuration
                            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile("ocelot.json", false, true)
                            .AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName}.json", false, true)
                            .AddEnvironmentVariables();
                    });
                });
    }
}
