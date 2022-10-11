using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.Todo.Api.Domain;
using Nexus.Todo.Api.Infrastructure;

namespace Nexus.Todo.Api.Features.Todo.Query
{
    public static class GetTodoList
    {
        public record Query() : IRequest<Result>;

        public record Result(IEnumerable<TodoId> Data);

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly TodoDbContext _db;
            public Handler(TodoDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var todos = await _db.Todos.ToListAsync(cancellationToken);

                return new Result(todos.Select(static todo => ToModel(todo)));
            }

            private static TodoId ToModel(Domain.Todo todo)
            {
                return todo.TodoId;
            }
        }

        public class Validator
        {

        }

    }
}
