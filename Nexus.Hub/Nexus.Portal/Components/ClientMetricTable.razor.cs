using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nexus.Portal.Components;

public partial class ClientMetricTable
{
    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    [CascadingParameter]
    public ClientStateProvider? ClientStateProvider { get; set; }

    private async void OpenDialog()
    {
        var parameters = new DialogParameters { ["clientId"] = ClientStateProvider?.Client?.Id };
        var dialog = await DialogService.ShowAsync<AddClientMetricDialog>("Add Measurement", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            if (ClientStateProvider is not null)
            {
                await ClientStateProvider.GetClientMetricsAsync();
            }
        }
    }
}
