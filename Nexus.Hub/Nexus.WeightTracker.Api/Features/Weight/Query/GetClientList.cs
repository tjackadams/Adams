using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure;

namespace Nexus.WeightTracker.Api.Features.Weight.Query;

public static class GetClientList
{
    public record Query() : IRequest<IResult>;

    public record Response(IReadOnlyCollection<ClientModel> Data);

    public record ClientModel(ClientId Id, string Name);

    public class Handler : IRequestHandler<Query, IResult>
    {
        private readonly WeightDbContext _db;

        public Handler(WeightDbContext db)
        {
            _db = db;
        }

        public async Task<IResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var clients = await _db.Clients
                .Select(static c => ToModel(c))
                .ToListAsync(cancellationToken);

            return TypedResults.Ok(new Response(clients));
        }

        private static ClientModel ToModel(Client client)
        {
            return new ClientModel(client.ClientId, client.Name);
        }
    }

}
