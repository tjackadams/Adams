using BlazorState;

namespace Nexus.Portal.Features.GlobalProgress;

public partial class ProgressState : State<ProgressState>
{
    public bool ShowProgress { get; private set; }

    public override void Initialize()
    {
        ShowProgress = false;
    }
}
