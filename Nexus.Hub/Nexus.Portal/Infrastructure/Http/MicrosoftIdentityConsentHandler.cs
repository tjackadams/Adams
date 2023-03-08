using Microsoft.Identity.Web;

namespace Nexus.Portal.Infrastructure.Http;

public class MicrosoftIdentityConsentHandler : DelegatingHandler
{
    private readonly MicrosoftIdentityConsentAndConditionalAccessHandler _handler;

    public MicrosoftIdentityConsentHandler(MicrosoftIdentityConsentAndConditionalAccessHandler handler)
    {
        _handler = handler;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            return base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _handler.HandleException(ex);
            throw;
        }
    }
}
