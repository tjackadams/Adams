using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nexus.Todo.Api.Domain;
using Nexus.Todo.Api.Features.Todo.Command;
using Nexus.Todo.Api.Features.Todo.Query;

namespace Nexus.Todo.Api.Infrastructure.Endpoints;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodo(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("todos", GetTodoListAsync)
            .WithTags(EndpointTags.Todo)
            .Produces<GetTodoList.Result>();

        routes.MapGet("todos/{id}", GetTodoItemsAsync)
            .WithTags(EndpointTags.Todo)
            .Produces<GetTodoItems.Result>()
            .ProducesValidationProblem();

        routes.MapPost("todos", CreateTodoListAsync)
            .WithTags(EndpointTags.Todo)
            .Produces<CreateTodoList.Result>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return routes;
    }

    private static async Task<IResult> GetTodoListAsync(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTodoList.Query(), cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetTodoItemsAsync([FromRoute] TodoId id, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTodoItems.Query(id), cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateTodoListAsync(CreateTodoList.Command command, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Created($"todos/{result.Id}", result);
    }

    private static class EndpointTags
    {
        public const string Todo = nameof(Todo);
    }
}
