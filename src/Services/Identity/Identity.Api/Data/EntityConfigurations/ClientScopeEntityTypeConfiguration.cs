using IdentityServer4;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Identity.Api.Data.EntityConfigurations
{
    public class ClientScopeEntityTypeConfiguration : IEntityTypeConfiguration<ClientScope>
    {
        public void Configure(EntityTypeBuilder<ClientScope> builder)
        {
            builder.HasData(
                new ClientScope {Id = 1, Scope = IdentityServerConstants.StandardScopes.OpenId, ClientId = 1},
                new ClientScope {Id = 2, Scope = IdentityServerConstants.StandardScopes.Profile, ClientId = 1},
                new ClientScope {Id = 3, Scope = IdentityServerConstants.StandardScopes.OfflineAccess, ClientId = 1},
                new ClientScope {Id = 4, Scope = "smokers", ClientId = 1},
                new ClientScope {Id = 5, Scope = "smokers", ClientId = 2},
                new ClientScope {Id = 6, Scope = "identity", ClientId = 3}
            );
        }
    }
}