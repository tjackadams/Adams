using BlazorState;
using MediatR;

namespace Nexus.Portal.Features.GlobalProgress;

public partial class GlobalProgressState
{
    public class IncrementGlobalProgressAction : IAction
    {
    }

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
            return Unit.Task;
        }
    }
}
