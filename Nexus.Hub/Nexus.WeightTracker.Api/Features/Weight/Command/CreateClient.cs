using FluentValidation;
using MediatR;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure;

namespace Nexus.WeightTracker.Api.Features.Weight.Command;

public class CreateClient
{
    public record Command(string Name) : IRequest<Result>;

    public record Result(ClientId Id, string Name);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly WeightDbContext _db;
        public Handler(WeightDbContext db)
        {
            _db = db;
        }
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var client = Client.Create(request.Name);

            var entry = _db.Clients.Add(client);

            await _db.SaveChangesAsync(cancellationToken);

            return ToModel(entry.Entity);
        }

        private static Result ToModel(Client client)
        {
            return new Result(client.ClientId, client.Name);
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(Client.MaximumNameLength);
        }
    }
}
