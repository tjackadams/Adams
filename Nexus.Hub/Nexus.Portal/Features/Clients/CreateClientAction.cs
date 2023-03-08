using BlazorState;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Features.Clients;

public partial class ClientState
{
    public record CreateClientAction(string Name) : IAction;

    public class CreateClientHandler : ActionHandler<CreateClientAction>
    {
        private readonly WeightTrackerClient _client;

        public CreateClientHandler(IStore aStore, WeightTrackerClient client)
            : base(aStore)
        {
            _client = client;
        }

        private ClientState ClientState => Store.GetState<ClientState>();

        public override async Task Handle(CreateClientAction aAction, CancellationToken aCancellationToken)
        {
            var client = await _client.CreateClientAsync(new CreateClientCommand(aAction.Name), aCancellationToken);

            ClientState.Clients.Add(client);
        }
    }
}
