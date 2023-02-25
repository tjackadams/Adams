using Fluxor;

namespace Nexus.Portal.Store.GlobalProgressUseCase;

[FeatureState]
public class GlobalProgressState
{
    public int RequestsInProgress {get; }
    private GlobalProgressState() { }

    public GlobalProgressState(int requestCounter)
    {
        RequestsInProgress = requestCounter;
    }
}
