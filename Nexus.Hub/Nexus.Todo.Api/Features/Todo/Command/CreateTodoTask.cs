using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.Todo.Api.Domain;
using Nexus.Todo.Api.Infrastructure;

namespace Nexus.Todo.Api.Features.Todo.Command;

public static class CreateTodoTask
{
    public record Command(TodoId Id, string Title) : IRequest<Unit?>;

    public class Handler : IRequestHandler<Command, Unit?>
    {
        private readonly TodoDbContext _db;
        public Handler(TodoDbContext db)
        {
            _db = db;
        }
        public async Task<Unit?> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = await _db.Todos
                .Include(t => t.Tasks)
                .Where(t => t.TodoId == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (todo == null)
            {
                return null;
            }

            todo.AddTask(request.Title);

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class Validator
    {

    }
}
