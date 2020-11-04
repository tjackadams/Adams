using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adams.Services.Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Adams.Services.Identity.Api.Data
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public async Task SeedAsync(ApplicationDbContext context, ILogger<ApplicationDbContextSeed> logger,
            int? retry = 0)
        {
            var retryForAvailability = retry.Value;

            try
            {
                if (!context.Users.Any())
                {
                    await context.Users.AddRangeAsync(GetDefaultUser());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}",
                        nameof(ApplicationDbContext));

                    await SeedAsync(context, logger, retryForAvailability);
                }
            }
        }

        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var user =
                new ApplicationUser
                {
                    Email = "demouser@microsoft.com",
                    Id = Guid.NewGuid().ToString(),
                    LastName = "DemoLastName",
                    Name = "DemoUser",
                    PhoneNumber = "1234567890",
                    UserName = "demouser@microsoft.com",
                    NormalizedEmail = "DEMOUSER@MICROSOFT.COM",
                    NormalizedUserName = "DEMOUSER@MICROSOFT.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

            return new List<ApplicationUser>
            {
                user
            };
        }
    }
}