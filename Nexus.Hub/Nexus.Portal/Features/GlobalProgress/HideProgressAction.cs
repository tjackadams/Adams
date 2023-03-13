using BlazorState;
using MediatR;

namespace Nexus.Portal.Features.GlobalProgress;

public partial class ProgressState
{
    public record struct HideProgressAction : IAction;

    public class HideProgressHandler : ActionHandler<HideProgressAction>
    {
        public HideProgressHandler(IStore store)
            : base(store)
        {
        }

        private ProgressState State => Store.GetState<ProgressState>();

        public override Task<Unit> Handle(HideProgressAction aAction, CancellationToken aCancellationToken)
        {
            State.ShowProgress = false;
            return Unit.Task;
        }
    }
}
