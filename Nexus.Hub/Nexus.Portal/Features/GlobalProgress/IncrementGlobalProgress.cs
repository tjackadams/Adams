using BlazorState;

namespace Nexus.Portal.Features.GlobalProgress;

public partial class GlobalProgressState
{
    public record struct IncrementGlobalProgressAction : IAction;

    public class IncrementGlobalProgressHandler : ActionHandler<IncrementGlobalProgressAction>
    {
        public IncrementGlobalProgressHandler(IStore store)
            : base(store)
        {
        }

        private GlobalProgressState State => Store.GetState<GlobalProgressState>();

        public override Task Handle(IncrementGlobalProgressAction aAction, CancellationToken aCancellationToken)
        {
            State.RequestsInProgress += 1;
            return Task.CompletedTask;
        }
    }
}
