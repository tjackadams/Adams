using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class
        ClientPostLogoutRedirectUriEntityTypeConfiguration : IEntityTypeConfiguration<ClientPostLogoutRedirectUri>
    {
        public void Configure(EntityTypeBuilder<ClientPostLogoutRedirectUri> builder)
        {
            builder.HasData(
                new ClientPostLogoutRedirectUri
                    {Id = 1, PostLogoutRedirectUri = "https://app.itadams.co.uk/signout-callback-oidc", ClientId = 1},
                new ClientPostLogoutRedirectUri
                    {Id = 2, PostLogoutRedirectUri = "https://api.itadams.co.uk/s/swagger/", ClientId = 2},
                new ClientPostLogoutRedirectUri
                    {Id = 3, PostLogoutRedirectUri = "https://identity.itadams.co.uk/swagger/", ClientId = 3});
        }
    }
}