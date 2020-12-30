using System;
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
                {
                    Id = 1, Name = "smokers", DisplayName = "Smokers Service", Enabled = true, NonEditable = true,
                    Created = new DateTime(2020, 12, 30, 13, 56, 40, 241, DateTimeKind.Utc)
                },
                new ApiResource
                {
                    Id = 2, Name = "identity", DisplayName = "Identity Administration Service", Enabled = true,
                    NonEditable = true, Created = new DateTime(2020, 12, 30, 13, 56, 40, 241, DateTimeKind.Utc)
                });
        }
    }
}