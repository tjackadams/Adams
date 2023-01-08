using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.Server;
using MudBlazor;

namespace Nexus.Portal.Components;

public partial class ClientMetricTable
{
    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    [CascadingParameter]
    public ClientStateProvider? ClientStateProvider { get; set; }

    private void OpenDialog()
    {
        var parameters = new DialogParameters { ["clientId"]=ClientStateProvider?.Client?.Id };
        var reference = DialogService.Show<AddClientMetricDialog>("Add Measurement", parameters);
    }
}
