using BlazorState;
using MediatR;

namespace Nexus.Portal.Features.GlobalProgress;

public partial class GlobalProgressState
{
    public record struct DecrementGlobalProgressAction : IAction;

    public class DecrementGlobalProgressHandler : ActionHandler<DecrementGlobalProgressAction>
    {
        public DecrementGlobalProgressHandler(IStore store)
            : base(store)
        {
        }

        private GlobalProgressState State => Store.GetState<GlobalProgressState>();

        public override Task Handle(DecrementGlobalProgressAction aAction, CancellationToken aCancellationToken)
        {
            State.RequestsInProgress -= 1;
            return Unit.Task;
        }
    }
}
