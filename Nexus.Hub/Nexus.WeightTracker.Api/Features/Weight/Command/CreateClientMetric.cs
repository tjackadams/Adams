﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure;

namespace Nexus.WeightTracker.Api.Features.Weight.Command;

public static class CreateClientMetric
{
    public record Command(
        [property: FromRoute] ClientId ClientId,
        [property: FromBody] Data Data) : IRequest<IResult>;

    public record Data(decimal RecordedValue, DateOnly RecordedDate);

    public class Handler : IRequestHandler<Command, IResult>
    {
        private readonly WeightDbContext _db;
        public Handler(WeightDbContext db)
        {
            _db = db;
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
                return TypedResults.ValidationProblem(result.Errors.ToDictionary(e => e.Metadata["PropetyName"].ToString(), e => new[] { e.Message }));
            }

            await _db.SaveChangesAsync(cancellationToken);

            return TypedResults.StatusCode(StatusCodes.Status201Created);
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            Transform(c => c.ClientId, c => c.Value)
                .GreaterThan(0);

            RuleFor(c => c.Data)
                .NotEmpty()
                .ChildRules(data =>
                {
                    data.RuleFor(d => d.RecordedValue)
                        .NotEmpty()
                        .GreaterThan(0)
                        .OverridePropertyName(nameof(Data.RecordedValue));

                    data.RuleFor(c => c.RecordedDate)
                        .NotEmpty()
                        .OverridePropertyName(nameof(Data.RecordedDate));
                });
        }
    }

}