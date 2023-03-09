using BlazorState;
using MediatR;

namespace Nexus.Portal.Features.GlobalProgress;

public partial class GlobalProgressState
{
    public record struct IncrementGlobalProgressAction : IAction;

    public class IncrementGlobalProgressHandler : ActionHandler<IncrementGlobalProgressAction>
    {
        public IncrementGlobalProgressHandler(IStore aStore)
            : base(aStore)
        {
        }

        private GlobalProgressState State => Store.GetState<GlobalProgressState>();

        public override Task<Unit> Handle(IncrementGlobalProgressAction aAction, CancellationToken aCancellationToken)
        {
            State.RequestsInProgress += 1;
            return Unit.Task;
        }
    }
}
