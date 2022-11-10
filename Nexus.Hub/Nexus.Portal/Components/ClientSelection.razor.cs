using Microsoft.AspNetCore.Components;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientSelection
{
    [Inject]
    public WeightTrackerClient TrackerClient { get; set; } = null!;

    [Parameter]
    public EventCallback<GetClientList_ClientModel> ClientChanged { get; set; }

    private GetClientList_Result? _clients;

    protected override async Task OnInitializedAsync()
    {
        _clients = await TrackerClient.GetClientListAsync();
    }

    private async Task OnClientChanged(GetClientList_ClientModel client)
    {
        await ClientChanged.InvokeAsync(client);
    }
}
