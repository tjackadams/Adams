using MediatR;
using Nexus.Todo.Api.Features.Todo.Query;

namespace Nexus.Todo.Api.Infrastructure.Endpoints;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodo(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("todos", GetTodoListAsync)
            .Produces<GetTodoList.Result>();

        return routes;
    }

    private static async Task<IResult> GetTodoListAsync(IMediator mediator, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetTodoList.Query(), cancellationToken);

        return Results.Ok(response);
    }
}
