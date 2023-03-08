using BlazorState;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Features.Clients;

public partial class ClientState
{
    public record SetCurrentClientAction(GetClientList_ClientModel Client) : IAction;

    public class SetCurrentClientHandler : ActionHandler<SetCurrentClientAction>
    {
        public SetCurrentClientHandler(IStore aStore)
            : base(aStore)
        {
        }

        private ClientState ClientState => Store.GetState<ClientState>();

        public override Task Handle(SetCurrentClientAction aAction, CancellationToken aCancellationToken)
        {
            ClientState.CurrentClient = aAction.Client;

            return Task.CompletedTask;
        }
    }
}
