using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Adams.Services.Identity.Api
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource("smokers", "Smokers Service"){ Scopes ={ "smokers"}}
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiScope> ApiScopes()
        {
            return new []
            {
                new ApiScope("smokers")
            };
        }

        public static IEnumerable<Client> Clients(Dictionary<string, string> clientUrls)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "webblazor",

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris =
                    {
                        $"{clientUrls["Blazor"]}/authentication/login-callback"
                    },
                    FrontChannelLogoutUri = $"{clientUrls["Blazor"]}/signout-oidc",
                    PostLogoutRedirectUris = {$"{clientUrls["Blazor"]}/signout-callback-oidc"},

                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess, 
                        "smokers"
                    },

                    AllowedCorsOrigins = {$"{clientUrls["Blazor"]}"},
                    RequirePkce = true,
                    RequireClientSecret = false
                },
                new Client
                {
                    ClientId = "smokingswaggerui",
                    ClientName = "Smoking Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["SmokingApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["SmokingApi"]}/swagger/" },

                    AllowedScopes = { "smokers" }
                }
            };
        }
    }
}