﻿using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientSelection
{
    [Inject]
    public WeightTrackerClient TrackerClient { get; set; } = null!;

    [Inject]
    public MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = null!;

    [Parameter]
    public EventCallback<GetClientList_ClientModel> ClientChanged { get; set; }

    private GetClientList_Result? _clients;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _clients = await TrackerClient.GetClientListAsync();
        }
        catch (Exception ex)
        {
            ConsentHandler.HandleException(ex);
        }

    }

    private async Task OnClientChanged(GetClientList_ClientModel client)
    {
        await ClientChanged.InvokeAsync(client);
    }
}
