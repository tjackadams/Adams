﻿using Microsoft.AspNetCore.Http.HttpResults;
using Nexus.WeightTracker.Api.Features.Weight.Command;
using Nexus.WeightTracker.Api.Features.Weight.Query;
using Nexus.WeightTracker.Api.Infrastructure.Authorization;
using Nexus.WeightTracker.Api.Infrastructure.Routing;

namespace Nexus.WeightTracker.Api.Features.Weight;

public static class ClientEndpoints
{
    public static IEndpointRouteBuilder MapClient(this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("clients")
            .RequireAuthorization(AuthorizationPolicyNames.Reader)
            .WithTags(EndpointTags.Client)
            .WithOpenApi();

        group.Get<GetClientList.Query>("")
            .Produces<GetClientList.Response>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        group.Post<CreateClient.Command>("")
            .Produces<CreateClient.Response>(StatusCodes.Status201Created)
            .RequireAuthorization(AuthorizationPolicyNames.Writer)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        group.Get<GetClientMetricList.Query>("{clientId}/metrics")
            .Produces<GetClientMetricList.Response>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        group.Post<CreateClientMetric.Command>("{clientId}/metrics")
            .Produces<Created>(StatusCodes.Status201Created)
            .RequireAuthorization(AuthorizationPolicyNames.Writer)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        return routes;
    }

    private static class EndpointTags
    {
        public const string Client = nameof(Client);
    }
}
