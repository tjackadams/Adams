﻿using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using MudBlazor;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientSelection
{
    [Inject]
    public WeightTrackerClient TrackerClient { get; set; } = null!;

    [Inject]
    public MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = null!;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    [Parameter]
    public EventCallback<GetClientList_ClientModel> ClientChanged { get; set; }

    [CascadingParameter]
    public ClientStateProvider? ClientStateProvider { get; set; }

    private async Task OnClientChanged(GetClientList_ClientModel client)
    {
        if (ClientStateProvider is not null)
        {
            await ClientStateProvider.SetClientAsync(client);
        }
    }

    private async void OpenDialog()
    {
        var dialog = await DialogService.ShowAsync<AddClientDialog>("Add Client");
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            try
            {
                ClientStateProvider?.GetClientsAsync();
            }
            catch (Exception ex)
            {
                ConsentHandler?.HandleException(ex);
            }
        }
    }
}
