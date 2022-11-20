using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.WeightTracker;

namespace Nexus.Portal.Components;

public partial class AddClientMetricDialog
{
    [Inject]
    public WeightTrackerClient TrackerClient { get; set; } = null!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    private ClientMetricModel _model = new ClientMetricModel();

    private void Cancel() => MudDialog.Cancel();

    public class ClientMetricModel
    {
        public int ClientId { get; set; }
        public DateTime? RecordedDate { get; set; }
    }
}
