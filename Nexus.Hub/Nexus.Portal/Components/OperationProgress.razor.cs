using Nexus.Portal.Features.GlobalProgress;

namespace Nexus.Portal.Components;

public partial class OperationProgress
{
    private GlobalProgressState State => GetState<GlobalProgressState>();
}
