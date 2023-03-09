using BlazorState;
using MediatR;
using Nexus.WeightTracker;

namespace Nexus.Portal.Features.Clients;

public partial class ClientState
{
    public record struct SetCurrentClientAction(Client Client) : IAction;

    public class SetCurrentClientHandler : ActionHandler<SetCurrentClientAction>
    {
        private readonly WeightTrackerClient _client;

        public SetCurrentClientHandler(IStore aStore, WeightTrackerClient client)
            : base(aStore)
        {
            _client = client;
        }

        private ClientState ClientState => Store.GetState<ClientState>();

        public override async Task<Unit> Handle(SetCurrentClientAction aAction, CancellationToken aCancellationToken)
        {
            var metrics = await _client.GetClientMetricListAsync(aAction.Client.ClientId, aCancellationToken);

            ClientState.CurrentClient = aAction.Client;
            ClientState.CurrentClientMetrics = metrics.Data
                .Select(m => new ClientMetric(m.ClientMetricId, m.RecordedDate, m.RecordedValueMetric)).ToList();

            return Unit.Value;
        }
    }
}
