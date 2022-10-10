using MassTransit.Mediator;
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
        var client = mediator.CreateRequestClient<GetTodoList.Query>();

        var response = await client.GetResponse<GetTodoList.Result>(new GetTodoList.Query(), cancellationToken);

        return Results.Ok(response.Message);
    }
}
