using Microsoft.AspNetCore.Components;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Pages;

public partial class WeightTracker
{
    [Inject]
    public ILogger<WeightTracker> Logger { get; set; } = null!;

    public async Task OnClientChanged(GetClientList_ClientModel client)
    {
        Logger.LogInformation("Client Selection Change: {ClientId}", client.Id);
    }
}
