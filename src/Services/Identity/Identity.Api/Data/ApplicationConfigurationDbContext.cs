using Adams.Services.Identity.Api.Data.EntityConfigurations;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Identity.Api.Data
{
    public class ApplicationConfigurationDbContext : ConfigurationDbContext<ApplicationConfigurationDbContext>
    {
        public ApplicationConfigurationDbContext(
            DbContextOptions<ApplicationConfigurationDbContext> options,
            ConfigurationStoreOptions storeOptions
        )
            : base(options, storeOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApiResourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiResourceScopeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiScopeClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiScopeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientCorsOriginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientGrantTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientPostLogoutRedirectUriEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientRedirectUriEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientScopeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientSecretEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityResourceClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityResourceEntityTypeConfiguration());
        }
    }
}