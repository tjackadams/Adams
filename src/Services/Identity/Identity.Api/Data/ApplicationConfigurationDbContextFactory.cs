using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Adams.Services.Identity.Api.Data
{
    public class ApplicationConfigurationDbContextFactory : IDesignTimeDbContextFactory<ApplicationConfigurationDbContext>
    {
        public ApplicationConfigurationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationConfigurationDbContext>();

            optionsBuilder.UseSqlServer(config.GetConnectionString("Identity"),
                o => o.MigrationsAssembly("Identity.Api"));

            return new ApplicationConfigurationDbContext(optionsBuilder.Options, new ConfigurationStoreOptions());
        }
    }
}
