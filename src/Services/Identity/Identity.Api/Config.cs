using System.Collections.Generic;
using IdentityServer4.Models;

namespace Adams.Services.Identity.Api
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
    
            return new ApiResource[]
            {

            };
            
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
    
                return new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                };
            
        }

        public static IEnumerable<ApiScope> ApiScopes()
        {
          
            return new ApiScope[]
                {
                };
            
        }

        public static IEnumerable<Client> Clients(Dictionary<string, string> clientUrls)
        {
            return new []
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
                        PostLogoutRedirectUris = { $"{clientUrls["Blazor"]}/signout-callback-oidc"},

                        AllowOfflineAccess = true,
                        AllowedScopes = {"openid", "profile", "scope2"},

                        AllowedCorsOrigins = { $"{clientUrls["Blazor"]}"},
                        RequirePkce = true,
                        RequireClientSecret = false
                    },
                };
            
        }
    }
}