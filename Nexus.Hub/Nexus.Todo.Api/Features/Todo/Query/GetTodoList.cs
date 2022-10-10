using MassTransit;
using Microsoft.EntityFrameworkCore;
using Nexus.Todo.Api.Domain;
using Nexus.Todo.Api.Infrastructure;

namespace Nexus.Todo.Api.Features.Todo.Query
{
    public static class GetTodoList
    {
        public record Query();

        public record Result(IReadOnlyCollection<TodoId> Todos);

        public class Handler : IConsumer<Query>
        {
            private readonly TodoDbContext _db;
            public Handler(TodoDbContext db)
            {
                _db = db;
            }
            public async Task Consume(ConsumeContext<Query> context)
            {
                var todos = await _db.Todos.ToListAsync(context.CancellationToken);

                await context.RespondAsync(new Result(todos.Select(t => t.TodoId).ToArray()));
            }
        }

        public class Validator
        {

        }

    }
}
