using MediatR;
using Nexus.Portal.Features.GlobalProgress;

namespace Nexus.Portal.Infrastructure.Http;

public class ShowOperationProgressMessageHandler : DelegatingHandler
{
    private readonly IMediator _mediator;

    public ShowOperationProgressMessageHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new GlobalProgressState.IncrementGlobalProgressAction(), cancellationToken).ConfigureAwait(false);

        HttpResponseMessage response;

        try
        {
            response = await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            await _mediator.Send(new GlobalProgressState.DecrementGlobalProgressAction(), cancellationToken).ConfigureAwait(false);
        }

        return response;
    }
}
