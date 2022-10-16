using FluentValidation;
using MediatR;
using Nexus.Todo.Api.Domain;
using Nexus.Todo.Api.Infrastructure;

namespace Nexus.Todo.Api.Features.Todo.Command;

public static class CreateTodoList
{
    public record Command(string? Title) : IRequest<Result>;

    public record Result(TodoId Id, string Title, DateTimeOffset CreatedTime);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly TodoDbContext _db;
        public Handler(TodoDbContext db)
        {
            _db = db;
        }
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = Domain.Todo.Create(request.Title!);

            var entity = _db.Todos.Add(todo);

            await _db.SaveChangesAsync(cancellationToken);

            return ToModel(entity.Entity);
        }

        private static Result ToModel(Domain.Todo todo)
        {
            return new Result(Id: todo.TodoId, Title: todo.Title, CreatedTime: todo.CreatedTime);
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Title)
                .NotEmpty()
                .MaximumLength(Domain.Todo.MaximumTitleLength);
        }
    }
}
