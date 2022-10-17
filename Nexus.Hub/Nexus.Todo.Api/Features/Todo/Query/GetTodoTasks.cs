using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.Todo.Api.Domain;
using Nexus.Todo.Api.Infrastructure;

namespace Nexus.Todo.Api.Features.Todo.Query;

public static class GetTodoTasks
{
    public record Query(TodoId Id) : IRequest<Result?>;

    public record Result(TodoId Id, string Title);

    public class Handler : IRequestHandler<Query, Result?>
    {
        private readonly TodoDbContext _db;
        public Handler(TodoDbContext db)
        {
            _db = db;
        }
        public async Task<Result?> Handle(Query request, CancellationToken cancellationToken)
        {
            var todo = await _db.Todos
                .Include(t => t.Tasks)
                .Where(t => t.TodoId == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            return todo is null ? null : ToModel(todo);
        }

        private static Result ToModel(Domain.Todo todo)
        {
            return new Result(todo.TodoId, todo.Title);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(q => q.Id)
                .NotEmpty();
        }
    }
}
