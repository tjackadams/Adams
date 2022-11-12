﻿using Nexus.WeightTracker.Api.Features.Weight.Command;
using Nexus.WeightTracker.Api.Features.Weight.Query;
using Nexus.WeightTracker.Api.Infrastructure.Routing;

namespace Nexus.WeightTracker.Api.Infrastructure.Endpoints;

public static class ClientEndpoints
{
    public static IEndpointRouteBuilder MapClient(this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("clients")
            .RequireAuthorization()
            .WithTags(EndpointTags.Client)
            .WithOpenApi();

        group.Get<GetClientList.Query>("")
            .Produces<GetClientList.Response>()
            .RequireAuthorization("Reader")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        group.Post<CreateClient.Command>("")
            .RequireAuthorization("Writer")
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        group.Get<GetClientMetricList.Query>("{clientId}/metrics")
            .Produces<GetClientMetricList.Response>()
            .RequireAuthorization("Reader")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        return routes;
    }

    private static class EndpointTags
    {
        public const string Client = nameof(Client);
    }
}
