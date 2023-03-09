using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Features.Weight.Models;
using Nexus.WeightTracker.Api.Infrastructure;

namespace Nexus.WeightTracker.Api.Features.Weight.Command;

public record struct CreateClientMetricCommand(double RecordedValue, DateOnly RecordedDate);

public static class CreateClientMetric
{
    public record Command(
        [property: FromRoute] ClientId ClientId,
        [property: FromBody] CreateClientMetricCommand Data) : IRequest<IResult>;

    public class Handler : IRequestHandler<Command, IResult>
    {
        private readonly WeightDbContext _db;
        private readonly IMapper _mapper;

        public Handler(WeightDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var client = await _db.Clients.Where(c => c.ClientId == request.ClientId)
                .Include(c => c.Metrics)
                .SingleOrDefaultAsync(cancellationToken);

            if (client is null)
            {
                return TypedResults.NotFound();
            }

            var result = client.AddMetric(request.Data.RecordedValue, request.Data.RecordedDate);

            if (result.IsFailed)
            {
                return TypedResults.ValidationProblem(
                    result.Errors.ToDictionary(e => e.Metadata["PropertyName"].ToString()!, e => new[] { e.Message }));
            }

            await _db.SaveChangesAsync(cancellationToken);

            var metric = client.Metrics
                .Single(m => m.RecordedDate == request.Data.RecordedDate);

            return TypedResults.Created($"/clients/{client.ClientId}/metrics/${metric.ClientMetricId}",
                _mapper.Map<ClientMetricViewModel>(metric));
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.ClientId.Value)
                .GreaterThan(0)
                .OverridePropertyName(nameof(Command.ClientId));

            RuleFor(c => c.Data)
                .NotEmpty()
                .ChildRules(data =>
                {
                    data.RuleFor(d => d.RecordedValue)
                        .NotEmpty()
                        .GreaterThan(0)
                        .OverridePropertyName(nameof(CreateClientMetricCommand.RecordedValue));

                    data.RuleFor(c => c.RecordedDate)
                        .NotEmpty()
                        .OverridePropertyName(nameof(CreateClientMetricCommand.RecordedDate));
                });
        }
    }
}
