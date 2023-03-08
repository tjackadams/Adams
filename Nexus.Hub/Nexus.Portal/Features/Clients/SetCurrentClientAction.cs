﻿using BlazorState;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Features.Clients;

public partial class ClientState
{
    public record SetCurrentClientAction(ClientViewModel Client) : IAction;

    public class SetCurrentClientHandler : ActionHandler<SetCurrentClientAction>
    {
        private readonly WeightTrackerClient _client;

        public SetCurrentClientHandler(IStore aStore, WeightTrackerClient client)
            : base(aStore)
        {
            _client = client;
        }

        private ClientState ClientState => Store.GetState<ClientState>();

        public override async Task Handle(SetCurrentClientAction aAction, CancellationToken aCancellationToken)
        {
            var metrics = await _client.GetClientMetricListAsync(aAction.Client.ClientId, aCancellationToken);

            ClientState.CurrentClient = aAction.Client;
            ClientState.CurrentClientMetrics = metrics.Data.ToList();
        }
    }
}
