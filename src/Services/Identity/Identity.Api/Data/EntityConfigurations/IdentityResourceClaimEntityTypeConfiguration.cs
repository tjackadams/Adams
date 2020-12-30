using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class IdentityResourceClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityResourceClaim>
    {
        public void Configure(EntityTypeBuilder<IdentityResourceClaim> builder)
        {
            builder.HasData(
                new IdentityResourceClaim {Id = 1, Type = "nickname", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 2, Type = "middle_name", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 3, Type = "given_name", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 4, Type = "family_name", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 5, Type = "name", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 6, Type = "preferred_username", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 7, Type = "profile", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 8, Type = "picture", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 9, Type = "website", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 10, Type = "gender", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 11, Type = "birthdate", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 12, Type = "zoneinfo", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 13, Type = "locale", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 14, Type = "updated_at", IdentityResourceId = 1},
                new IdentityResourceClaim {Id = 15, Type = "sub", IdentityResourceId = 2}
            );
        }
    }
}