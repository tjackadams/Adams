using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ClientRedirectUriEntityTypeConfiguration : IEntityTypeConfiguration<ClientRedirectUri>
    {
        public void Configure(EntityTypeBuilder<ClientRedirectUri> builder)
        {
            builder.HasData(
                new ClientRedirectUri
                    {Id = 1, RedirectUri = "https://app.itadams.co.uk/authentication/login-callback", ClientId = 1},
                new ClientRedirectUri
                    {Id = 2, RedirectUri = "https://api.itadams.co.uk/s/swagger/oauth2-redirect.html", ClientId = 2},
                new ClientRedirectUri
                    {Id = 3, RedirectUri = "https://identity.itadams.co.uk/swagger/oauth2-redirect.html", ClientId = 3}
            );
        }
    }
}