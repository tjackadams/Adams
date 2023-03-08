using AutoMapper;
using FluentValidation;
using MediatR;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Features.Weight.Models;
using Nexus.WeightTracker.Api.Infrastructure;
using Nexus.WeightTracker.Api.Infrastructure.Authorization;

namespace Nexus.WeightTracker.Api.Features.Weight.Command;

public class CreateClient
{
    public record Command(string Name) : IRequest<IResult>;

    public class Handler : IRequestHandler<Command, IResult>
    {
        private readonly WeightDbContext _db;
        private readonly IdentityClaimProvider _identity;
        private readonly IMapper _mapper;

        public Handler(WeightDbContext db, IdentityClaimProvider identity, IMapper mapper)
        {
            _db = db;
            _identity = identity;
            _mapper = mapper;
        }
        public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var client = new Client(request.Name, _identity.GetObjectId());

            var entry = _db.Clients.Add(client);

            await _db.SaveChangesAsync(cancellationToken);

            return TypedResults.Created($"/clients/{entry.Entity.ClientId}", _mapper.Map<ClientViewModel>(entry.Entity));
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
