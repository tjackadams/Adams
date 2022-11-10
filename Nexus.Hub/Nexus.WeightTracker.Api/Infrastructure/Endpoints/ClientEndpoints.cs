using MediatR;
using Nexus.WeightTracker.Api.Features.Weight.Command;
using Nexus.WeightTracker.Api.Features.Weight.Query;

namespace Nexus.WeightTracker.Api.Infrastructure.Endpoints;

public static class ClientEndpoints
{
    public static IEndpointRouteBuilder MapClient(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("clients");
        group.WithTags(EndpointTags.Client);

        group.MapGet("", GetClientListAsync)
            .WithName(nameof(GetClientListAsync))
            .Produces<GetClientList.Result>();

        group.MapPost("", CreateClientAsync)
            .WithName(nameof(CreateClientAsync))
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
