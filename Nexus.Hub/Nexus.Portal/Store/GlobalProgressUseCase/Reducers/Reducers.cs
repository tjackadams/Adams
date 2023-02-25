using Fluxor;

namespace Nexus.Portal.Store.GlobalProgressUseCase.Reducers;

public static class Reducers
{
    [ReducerMethod(typeof(IncrementGlobalProgressAction))]
    public static GlobalProgressState ReduceIncrementGlobalProgressAction(GlobalProgressState state)
    {
        return new GlobalProgressState(state.RequestsInProgress + 1);
    }

    [ReducerMethod(typeof(DecrementGlobalProgressAction))]
    public static GlobalProgressState ReduceDecrementGlobalProgressAction(GlobalProgressState state)
    {
        return new GlobalProgressState(state.RequestsInProgress - 1);
    }
}
