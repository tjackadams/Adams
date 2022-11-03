using MediatR;
using Nexus.WeightTracker.Api.Features.Weight.Query;

namespace Nexus.WeightTracker.Api.Infrastructure.Endpoints;

public static class ClientEndpoints
{
    public static IEndpointRouteBuilder MapClient(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("clients", GetClientListAsync)
            .WithTags(EndpointTags.Client)
            .Produces<GetClientList.Result>();

        return routes;
    }

    private static async Task<IResult> GetClientListAsync(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetClientList.Query(), cancellationToken);

        return Results.Ok(result);
    }

    private static class EndpointTags
    {
        public const string Client = nameof(Client);
    }
}
