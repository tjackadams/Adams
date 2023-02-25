using Fluxor;
using Nexus.Portal.Store.GlobalProgressUseCase;

namespace Nexus.Portal.Infrastructure.Http;

public class ShowOperationProgressMessageHandler : DelegatingHandler
{
    private readonly IDispatcher _dispatcher;

    public ShowOperationProgressMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _dispatcher.Dispatch(new IncrementGlobalProgressAction());

        HttpResponseMessage response;

        try
        {
            response = await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            _dispatcher.Dispatch(new DecrementGlobalProgressAction());
        }

        return response;
    }
}
