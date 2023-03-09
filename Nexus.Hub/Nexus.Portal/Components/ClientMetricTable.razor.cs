using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.Portal.Features.Clients;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientMetricTable
{
    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    private ClientState ClientState => GetState<ClientState>();

    private Client? CurrentClient => ClientState.CurrentClient;

    private List<ClientMetricViewModel> CurrentClientMetrics => ClientState.CurrentClientMetrics;

    private async void OpenDialog()
    {
        if (CurrentClient is not null)
        {
            var parameters = new DialogParameters { ["clientId"] = CurrentClient.ClientId };
            var dialog = await DialogService.ShowAsync<AddClientMetricDialog>("Add Measurement", parameters);
            _ = await dialog.Result;
        }
    }
}
