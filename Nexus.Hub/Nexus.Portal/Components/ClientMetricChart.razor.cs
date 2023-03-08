using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientMetricChart
{
    private int? _previousClientId;
    private List<ChartSeries>? _series;

    private bool _shouldRender;
    private string[]? _xAxisLabels;

    private int Index = -1;

    [Parameter]
    public ClientViewModel Client { get; set; } = null!;

    [Parameter]
    public List<ClientMetricViewModel> Metrics { get; set; } = null!;

    protected override void OnParametersSet()
    {
        _shouldRender = Client.ClientId != _previousClientId;

        _previousClientId = Client.ClientId;

        var data = Metrics
            .OrderByDescending(m => m.RecordedDate)
            .Take(12)
            .ToArray();

        _xAxisLabels = data.Select(m => m.RecordedDate.ToShortDateString()).ToArray();

        _series = new List<ChartSeries>
        {
            new() { Name = Client.Name, Data = data.Select(m => m.RecordedValueMetric).ToArray() }
        };
    }

    protected override bool ShouldRender()
    {
        return _shouldRender;
    }
}
