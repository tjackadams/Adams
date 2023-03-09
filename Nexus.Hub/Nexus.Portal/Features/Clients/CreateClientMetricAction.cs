using BlazorState;
using MediatR;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Features.Clients;

public partial class ClientState
{
    public record struct CreateClientMetricAction(int ClientId, DateOnly RecordedDate, double RecordedValue) : IAction;

    public class CreateClientMetricHandler : ActionHandler<CreateClientMetricAction>
    {
        private readonly WeightTrackerClient _client;

        public CreateClientMetricHandler(IStore aStore, WeightTrackerClient client)
            : base(aStore)
        {
            _client = client;
        }

        private ClientState ClientState => Store.GetState<ClientState>();

        public override async Task<Unit> Handle(CreateClientMetricAction aAction, CancellationToken aCancellationToken)
        {
            var metric = await _client.CreateClientMetricAsync(aAction.ClientId,
                new CreateClientMetricCommand(aAction.RecordedDate, aAction.RecordedValue),
                aCancellationToken);

            ClientState.CurrentClientMetrics.Add(metric);

            return Unit.Value;
        }
    }
}
