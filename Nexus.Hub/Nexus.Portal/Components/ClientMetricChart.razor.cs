using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nexus.Portal.Components;

public partial class ClientMetricChart
{
    private List<ChartSeries>? _series;
    private string[]? _xAxisLabels;

    private int Index = -1;

    [CascadingParameter]
    public ClientStateProvider? ClientStateProvider { get; set; }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            return Task.CompletedTask;
        }

        if (ClientStateProvider is not null)
        {
            if (ClientStateProvider.Metrics is not null)
            {
                var data = ClientStateProvider.Metrics.Data
                    .OrderByDescending(m => m.RecordedDate)
                    .Take(12)
                    .ToArray();

                _xAxisLabels = data.Select(m => m.RecordedDate.ToShortDateString()).ToArray();

                _series = new List<ChartSeries>
                {
                    new()
                    {
                        Name = ClientStateProvider.Client.Name,
                        Data = data.Select(m => m.RecordedValueMetric).ToArray()
                    }
                };

                StateHasChanged();
            }
        }

        return Task.CompletedTask;
    }
}
