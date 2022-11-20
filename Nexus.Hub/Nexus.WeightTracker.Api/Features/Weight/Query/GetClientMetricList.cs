﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure;

namespace Nexus.WeightTracker.Api.Features.Weight.Query;

public static class GetClientMetricList
{
    public record Query(ClientId ClientId) : IRequest<IResult>;

    public record Response(IReadOnlyCollection<ClientMetric> Data);

    public record ClientMetric(ClientMetricId ClientMetricId, decimal RecordedValueMetric,
        decimal RecordedValueImperial, DateOnly RecordedDate);
    public class Handler : IRequestHandler<Query, IResult>
    {
        private readonly WeightDbContext _db;
        public Handler(WeightDbContext db)
        {
            _db = db;
        }
        public async Task<IResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var client = await _db.Clients
                .Where(c => c.ClientId == request.ClientId)
                .Include(c => c.Metrics)
                .SingleOrDefaultAsync(cancellationToken);

            if (client is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(new Response(client.Metrics.Select(static m => ToModel(m)).ToArray()));
        }

        private static ClientMetric ToModel(Domain.ClientMetric metric)
        {
            return new ClientMetric(
                ClientMetricId: metric.ClientMetricId,
                RecordedValueMetric: metric.RecordedValue,
                RecordedValueImperial: metric.RecordedValue * 0.15747M,
                RecordedDate: metric.RecordedDate
                );
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {

        }
    }
}
