using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasData(
                new Client
                {
                    Id = 1, Enabled = true, ClientId = "webblazor", ClientName = "Blazor Client Application",
                    FrontChannelLogoutUri = "https://app.itadams.co.uk/signout-oidc", AllowOfflineAccess = true,
                    RequireClientSecret = false
                },
                new Client
                {
                    Id = 2, Enabled = true, ClientId = "smokingswaggerui", ClientName = "Smoking Swagger UI",
                    AllowAccessTokensViaBrowser = true, RequireClientSecret = false
                },
                new Client
                {
                    Id = 3, Enabled = true, ClientId = "identityswaggerui",
                    ClientName = "Identity Administration Swagger UI", AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false
                }
            );
        }
    }
}