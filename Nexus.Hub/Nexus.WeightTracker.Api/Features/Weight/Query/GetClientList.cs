using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure;

namespace Nexus.WeightTracker.Api.Features.Weight.Query;

public static class GetClientList
{
    public record Query() : IRequest<Result>;

    public record Result(IReadOnlyCollection<ClientModel> Data);

    public record ClientModel(ClientId Id, string Name);

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly WeightDbContext _db;

        public Handler(WeightDbContext db)
        {
            _db = db;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var clients = await _db.Clients
                .Select(c => ToModel(c))
                .ToListAsync(cancellationToken);

            return new Result(clients);
        }

        private static ClientModel ToModel(Client client)
        {
            return new ClientModel(client.ClientId, client.Name);
        }
    }

}
