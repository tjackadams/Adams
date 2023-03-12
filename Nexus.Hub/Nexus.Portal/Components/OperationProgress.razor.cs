using Nexus.Portal.Features.GlobalProgress;

namespace Nexus.Portal.Components;

public partial class OperationProgress
{
    private ProgressState State => GetState<ProgressState>();

    private bool ShowProgress => State.ShowProgress;
}
