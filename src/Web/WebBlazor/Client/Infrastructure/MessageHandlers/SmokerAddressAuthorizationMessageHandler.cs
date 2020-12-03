using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace WebBlazor.Client.Infrastructure.MessageHandlers
{
    public class SmokerAddressAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public SmokerAddressAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation,
            IOptions<AppSettings> settings)
            : base(provider, navigation)
        {
            ConfigureHandler(
                settings.Value.Smoker.AuthorizedUrls,
                settings.Value.Smoker.Scopes
            );
        }
    }
}