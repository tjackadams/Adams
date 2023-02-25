using Fluxor;
using Microsoft.AspNetCore.Components;
using Nexus.Portal.Services;
using Nexus.Portal.Store.GlobalProgressUseCase;

namespace Nexus.Portal.Components;

public partial class OperationProgress
{
    [Inject]
    public IState<GlobalProgressState> GlobalProgressState { get; set; } = null!;
}
