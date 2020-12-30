using IdentityModel;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ApiScopeClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApiScopeClaim>
    {
        public void Configure(EntityTypeBuilder<ApiScopeClaim> builder)
        {
            builder.HasData(
                new ApiScopeClaim {Id = 1, Type = JwtClaimTypes.Role, ScopeId = 1}
            );
        }
    }
}