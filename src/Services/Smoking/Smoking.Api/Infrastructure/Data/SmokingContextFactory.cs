using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Adams.Services.Smoking.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Adams.Services.Smoking.Api.Infrastructure.Data
{
    public class SmokingContextFactory: IDesignTimeDbContextFactory<SmokingContext>
    {
        public SmokingContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SmokingContext>();

            optionsBuilder.UseSqlServer(config.GetConnectionString("Smoking"),o => o.MigrationsAssembly("Smoking.Migrations"));

            return new SmokingContext(optionsBuilder.Options);
        }
    }
}
