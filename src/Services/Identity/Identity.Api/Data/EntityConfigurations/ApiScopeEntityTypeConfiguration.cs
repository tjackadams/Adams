using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ApiScopeEntityTypeConfiguration : IEntityTypeConfiguration<ApiScope>
    {
        public void Configure(EntityTypeBuilder<ApiScope> builder)
        {
            builder.HasData(
                new ApiScope {Id = 1, Name = "identity", Enabled = true, ShowInDiscoveryDocument = true},
                new ApiScope {Id = 2, Name = "smokers", Enabled = true, ShowInDiscoveryDocument = true}
            );
        }
    }
}