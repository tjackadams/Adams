using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ApiResourceEntityTypeConfiguration : IEntityTypeConfiguration<ApiResource>
    {
        public void Configure(EntityTypeBuilder<ApiResource> builder)
        {
            builder.HasData(
                new ApiResource
                    {Id = 1, Name = "smokers", DisplayName = "Smokers Service", Enabled = true, NonEditable = true},
                new ApiResource
                {
                    Id = 2, Name = "identity", DisplayName = "Identity Administration Service", Enabled = true,
                    NonEditable = true
                });
        }
    }
}