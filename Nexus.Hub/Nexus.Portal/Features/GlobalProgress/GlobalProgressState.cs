using BlazorState;

namespace Nexus.Portal.Features.GlobalProgress;

public partial  class GlobalProgressState : State<GlobalProgressState>
{
    public int RequestsInProgress { get; private set;}

    public override void Initialize()
    {
        RequestsInProgress = 0;
    }
}
