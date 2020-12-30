using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ClientSecretEntityTypeConfiguration : IEntityTypeConfiguration<ClientSecret>
    {
        public void Configure(EntityTypeBuilder<ClientSecret> builder)
        {
            builder.HasData(
                new ClientSecret {Id = 1, Value = "secret".Sha256(), ClientId = 2},
                new ClientSecret {Id = 2, Value = "secret".Sha256(), ClientId = 3}
            );
        }
    }
}