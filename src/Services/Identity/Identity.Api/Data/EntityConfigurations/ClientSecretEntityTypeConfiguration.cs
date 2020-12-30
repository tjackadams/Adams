using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ClientSecretEntityTypeConfiguration : IEntityTypeConfiguration<ClientSecret>
    {
        public void Configure(EntityTypeBuilder<ClientSecret> builder)
        {
        }
    }
}