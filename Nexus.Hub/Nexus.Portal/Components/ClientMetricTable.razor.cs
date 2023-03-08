using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientMetricTable
{
    private int? _previousClientId;

    private bool _shouldRender;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    [Parameter]
    public ClientViewModel Client { get; set; } = null!;

    [Parameter]
    public List<ClientMetricViewModel> Metrics { get; set; } = null!;

    private async void OpenDialog()
    {
        var parameters = new DialogParameters { ["clientId"] = Client.ClientId };
        var dialog = await DialogService.ShowAsync<AddClientMetricDialog>("Add Measurement", parameters);
        _ = await dialog.Result;
    }

    protected override void OnParametersSet()
    {
        _shouldRender = Client?.ClientId != _previousClientId;

        _previousClientId = Client?.ClientId ?? 0;
    }

    protected override bool ShouldRender()
    {
        return _shouldRender;
    }
}
