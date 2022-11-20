using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientStateProvider
{
    [Inject]
    public WeightTrackerClient TrackerClient { get; set; } = null!;

    [Inject]
    public MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = null!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public GetClientList_ClientModel? Client { get; private set; }

    public GetClientMetricList_Response? Metrics { get; private set; }

    public async Task SetClientAsync(GetClientList_ClientModel client)
    {
        Client = client;

        try
        {
            Metrics = await TrackerClient.GetClientMetricListAsync(client.Id);

            StateHasChanged();
        }
        catch (Exception ex)
        {
            ConsentHandler.HandleException(ex);
        }

    }
}
