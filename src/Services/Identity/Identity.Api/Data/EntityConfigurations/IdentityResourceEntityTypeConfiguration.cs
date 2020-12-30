using IdentityServer4;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class IdentityResourceEntityTypeConfiguration : IEntityTypeConfiguration<IdentityResource>
    {
        public void Configure(EntityTypeBuilder<IdentityResource> builder)
        {
            builder.HasData(
                new IdentityResource
                {
                    Id = 1, Name = IdentityServerConstants.StandardScopes.Profile, DisplayName = "User profile",
                    Description = "Your user profile information (first name, last name, etc.)", Required = false,
                    Emphasize = true, Enabled = true, ShowInDiscoveryDocument = true
                },
                new IdentityResource
                {
                    Id = 2, Name = IdentityServerConstants.StandardScopes.OpenId, DisplayName = "Your user identifier",
                    Required = true, Emphasize = false, ShowInDiscoveryDocument = true, Enabled = true
                }
            );
        }
    }
}