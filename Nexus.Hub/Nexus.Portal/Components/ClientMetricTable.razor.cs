using Microsoft.AspNetCore.Components;
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
        var reference = DialogService.Show<AddClientMetricDialog>("Add Measurement");
    }
}
