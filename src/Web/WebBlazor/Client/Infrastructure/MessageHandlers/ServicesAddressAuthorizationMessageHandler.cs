using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace WebBlazor.Client.Infrastructure.MessageHandlers
{
    public class ServicesAddressAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public ServicesAddressAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation,
            IOptions<AppSettings> settings)
            : base(provider, navigation)
        {
            ConfigureHandler(
                settings.Value.AuthorizedUrls,
                settings.Value.Scopes
            );
        }
    }
}