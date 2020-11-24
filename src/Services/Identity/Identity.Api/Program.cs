using System;
using System.IO;
using System.Linq;
using Adams.Services.Identity.Api.Data;
using Adams.Services.Identity.Api.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;

namespace Adams.Services.Identity.Api
{
    public class Program
    {
        private static readonly string Namespace = typeof(Program).Namespace;

        private static readonly string AppName =
            Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = BuildWebHost(configuration, args);

                Log.Information("Applying migrations ({ApplicationContext})...", AppName);
                host.MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
                    .MigrateDbContext<ApplicationDbContext>(async (_, services) =>
                    {
                        using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                        {
                            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                            var tom = await userManager.FindByEmailAsync("tjackadams@itadams.co.uk");
                            if (tom == null)
                            {
                                tom = new ApplicationUser
                                {
                                    UserName = "tjackadams@itadams.co.uk",
                                    Email = "tjackadams@itadams.co.uk",
                                    EmailConfirmed = true
                                };

                                var result = await userManager.CreateAsync(tom, "Jasper2854");
                                if (!result.Succeeded)
                                {
                                    throw new Exception(result.Errors.First().Description);
                                }

                                result = await userManager.AddClaimsAsync(tom, new[]
                                {
                                    new System.Security.Claims.Claim(JwtClaimTypes.Name, "Thomas Adams"),
                                    new System.Security.Claims.Claim(JwtClaimTypes.GivenName, "Thomas"),
                                    new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, "Adams"),
                                    new System.Security.Claims.Claim(JwtClaimTypes.WebSite,
                                        "https://blog.itadams.co.uk")
                                });
                                if (!result.Succeeded)
                                {
                                    throw new Exception(result.Errors.First().Description);
                                }
                            }
                        }
                    });

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();
        }

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}