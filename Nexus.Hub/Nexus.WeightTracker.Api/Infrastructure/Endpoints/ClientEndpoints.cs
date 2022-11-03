using MediatR;
using Nexus.WeightTracker.Api.Features.Weight.Command;
using Nexus.WeightTracker.Api.Features.Weight.Query;

namespace Nexus.WeightTracker.Api.Infrastructure.Endpoints;

public static class ClientEndpoints
{
    public static IEndpointRouteBuilder MapClient(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("clients", GetClientListAsync)
            .WithTags(EndpointTags.Client)
            .Produces<GetClientList.Result>();

        routes.MapPost("clients", CreateClientAsync)
            .WithTags(EndpointTags.Client)
            .Produces<CreateClient.Result>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return routes;
    }

    private static async Task<IResult> GetClientListAsync(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetClientList.Query(), cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateClientAsync(CreateClient.Command command, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Created($"/clients/{result.Id}", result);
    }

    private static class EndpointTags
    {
        public const string Client = nameof(Client);
    }
}
