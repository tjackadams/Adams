using IdentityModel;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ClientGrantTypeEntityTypeConfiguration : IEntityTypeConfiguration<ClientGrantType>
    {
        public void Configure(EntityTypeBuilder<ClientGrantType> builder)
        {
            builder.HasData(
                new ClientGrantType {Id = 1, GrantType = OidcConstants.GrantTypes.AuthorizationCode, ClientId = 1},
                new ClientGrantType {Id = 2, GrantType = OidcConstants.GrantTypes.AuthorizationCode, ClientId = 2},
                new ClientGrantType {Id = 3, GrantType = OidcConstants.GrantTypes.AuthorizationCode, ClientId = 3}
            );
        }
    }
}