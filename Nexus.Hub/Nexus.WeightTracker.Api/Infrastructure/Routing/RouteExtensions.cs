using MediatR;

namespace Nexus.WeightTracker.Api.Infrastructure.Routing;

public static class RouteExtensions
{
    public static RouteHandlerBuilder Get<TRequest>(this RouteGroupBuilder group, string template)
        where TRequest : IRequest<IResult>
    {
        return group.MapGet(template,
            ([AsParameters] TRequest request, IMediator mediator, CancellationToken cancellationToken) =>
                mediator.Send(request, cancellationToken))
            .WithName(typeof(TRequest).DeclaringType?.Name ?? typeof(TRequest).Name);
    }

    public static RouteHandlerBuilder Post<TRequest>(this RouteGroupBuilder group, string template)
    where TRequest : IRequest<IResult>
    {
        return group.MapPost(template,
                    ([AsParameters] TRequest request, IMediator mediator, CancellationToken cancellationToken) =>
                mediator.Send(request, cancellationToken))
            .WithName(typeof(TRequest).DeclaringType?.Name ?? typeof(TRequest).Name);
    }
}
