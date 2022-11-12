using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Features.Weight.Command;
using Nexus.WeightTracker.Api.Features.Weight.Query;

namespace Nexus.WeightTracker.Api.Infrastructure.Endpoints;

public static class ClientEndpoints
{
    public static IEndpointRouteBuilder MapClient(this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("clients")
            .RequireAuthorization()
            .WithTags(EndpointTags.Client);

        group.MapGet("", GetClientListAsync)
            .RequireAuthorization("Reader")
            .WithName(nameof(GetClientListAsync))
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        group.MapPost("", CreateClientAsync)
            .RequireAuthorization("Writer")
            .WithName(nameof(CreateClientAsync))
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        group.MapGet("{clientId}/metrics", GetClientMetricListAsync)
            .RequireAuthorization("Reader");

        return routes;
    }

    private static async Task<Ok<GetClientList.Result>> GetClientListAsync(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetClientList.Query(), cancellationToken);

        return TypedResults.Ok(result);
    }

    private static async Task<Created<CreateClient.Result>> CreateClientAsync(CreateClient.Command command, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.Created($"/clients/{result.Id}", result);
    }

    private static async Task<Ok<GetClientMetricList.Result>> GetClientMetricListAsync(ClientId clientId,
        IMediator mediator, CancellationToken cancellationToken

    )
    {
        var result = await mediator.Send(new GetClientMetricList.Query(clientId), cancellationToken);

        return TypedResults.Ok(result);
    }

    private static class EndpointTags
    {
        public const string Client = nameof(Client);
    }
}
