﻿using FluentValidation;
using MediatR;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure;
using Nexus.WeightTracker.Api.Infrastructure.Authorization;

namespace Nexus.WeightTracker.Api.Features.Weight.Command;

public class CreateClient
{
    public record Command(string Name) : IRequest<Result>;

    public record Result(ClientId Id, string Name);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly WeightDbContext _db;
        private readonly IdentityClaimProvider _identity;
        public Handler(WeightDbContext db, IdentityClaimProvider identity)
        {
            _db = db;
            _identity = identity;
        }
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var client = new Client(request.Name, _identity.GetObjectId());

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
