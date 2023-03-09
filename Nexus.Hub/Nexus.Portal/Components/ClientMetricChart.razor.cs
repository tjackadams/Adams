using MudBlazor;
using Nexus.Portal.Features.Clients;

namespace Nexus.Portal.Components;

public partial class ClientMetricChart
{
    private List<ChartSeries>? _series;
    private string[]? _xAxisLabels;
    private int Index = -1;

    private ClientState ClientState => GetState<ClientState>();
    private Client? CurrentClient => ClientState.CurrentClient;

    private List<ClientMetric> CurrentClientMetrics => ClientState.CurrentClientMetrics;

    protected override void OnInitialized()
    {
        if (CurrentClient is not null)
        {
            var data = CurrentClientMetrics
                .OrderBy(m => m.RecordedDate)
                .Take(12)
                .ToArray();

            _xAxisLabels = data.Select(m => m.RecordedDate.ToShortDateString()).ToArray();

            _series = new List<ChartSeries>
        {
            new() { Name = CurrentClient.Name, Data = data.Select(m => m.RecordedValueMetric).ToArray() }
        };
        }
    }
}
