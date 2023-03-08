using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Features.Weight.Models;
using Nexus.WeightTracker.Api.Infrastructure;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Nexus.WeightTracker.Api.Features.Weight.Query;

public static class GetClientMetricList
{
    public record Query(ClientId ClientId) : IRequest<IResult>;

    public record Response(IReadOnlyCollection<ClientMetricViewModel> Data);

    public record ClientMetric(ClientMetricId ClientMetricId, double RecordedValueMetric, DateOnly RecordedDate);

    public class Handler : IRequestHandler<Query, IResult>
    {
        private readonly WeightDbContext _db;
        private readonly IConfigurationProvider _provider;

        public Handler(WeightDbContext db, IConfigurationProvider provider)
        {
            _db = db;
            _provider = provider;
        }

        public async Task<IResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var metrics = await _db.ClientMetrics
                .Where(c => c.ClientId == request.ClientId)
                .ProjectTo<ClientMetricViewModel>(_provider)
                .ToListAsync(cancellationToken);

            return TypedResults.Ok(new Response(metrics));
        }
    }

    public class Validator : AbstractValidator<Query>
    {
    }
}
