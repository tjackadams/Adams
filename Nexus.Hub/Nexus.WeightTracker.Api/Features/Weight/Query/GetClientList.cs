using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.WeightTracker.Api.Features.Weight.Models;
using Nexus.WeightTracker.Api.Infrastructure;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Nexus.WeightTracker.Api.Features.Weight.Query;

public static class GetClientList
{
    public record Query : IRequest<IResult>;

    public record Response(IReadOnlyCollection<ClientViewModel> Data);

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
            var clients = await _db.Clients
                .ProjectTo<ClientViewModel>(_provider)
                .ToListAsync(cancellationToken);

            return TypedResults.Ok(new Response(clients));
        }
    }
}
