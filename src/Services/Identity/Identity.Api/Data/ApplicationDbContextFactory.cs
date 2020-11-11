using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Adams.Services.Identity.Api.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            SqlServerDbContextOptionsExtensions.UseSqlServer((DbContextOptionsBuilder) optionsBuilder, config.GetConnectionString("Identity"), o => o.MigrationsAssembly("Identity.Api"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}