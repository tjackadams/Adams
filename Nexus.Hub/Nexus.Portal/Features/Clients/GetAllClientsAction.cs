using BlazorState;
using Nexus.WeightTracker;

namespace Nexus.Portal.Features.Clients;

public partial class ClientState
{
    public record struct GetAllClientsAction : IAction;

    public class GetAllClientsHandler : ActionHandler<GetAllClientsAction>
    {
        private readonly WeightTrackerClient _client;

        public GetAllClientsHandler(IStore aStore, WeightTrackerClient client)
            : base(aStore)
        {
            _client = client;
        }

        private ClientState ClientState => Store.GetState<ClientState>();

        public override async Task Handle(GetAllClientsAction aAction, CancellationToken aCancellationToken)
        {
            var clients = await _client.GetClientListAsync(aCancellationToken);

            ClientState.Clients = clients.Data.ToList();
        }
    }
}
