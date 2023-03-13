using BlazorState;
using MediatR;

namespace Nexus.Portal.Features.GlobalProgress;

public partial class ProgressState
{
    public record struct ShowProgressAction : IAction;

    public class ShowProgressHandler : ActionHandler<ShowProgressAction>
    {
        public ShowProgressHandler(IStore aStore)
            : base(aStore)
        {
        }

        private ProgressState State => Store.GetState<ProgressState>();

        public override Task<Unit> Handle(ShowProgressAction aAction, CancellationToken aCancellationToken)
        {
            State.ShowProgress = true;
            return Unit.Task;
        }
    }
}
