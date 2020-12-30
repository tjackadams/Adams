using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ApiResourceScopeEntityTypeConfiguration : IEntityTypeConfiguration<ApiResourceScope>
    {
        public void Configure(EntityTypeBuilder<ApiResourceScope> builder)
        {
            builder.HasData(
                new ApiResourceScope {Id = 1, Scope = "smokers", ApiResourceId = 1},
                new ApiResourceScope {Id = 2, Scope = "identity", ApiResourceId = 2});
        }
    }
}