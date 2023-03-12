using BlazorState;
using MudBlazor;

namespace Nexus.Portal.Features.Clients;

/// <summary>
///     Client is used in a select list so it needs equality operators. Currently the ClientViewModel that
///     comes from the api is a class, so we map this to a record as a workaround.
///     <see href="https://github.com/RicoSuter/NSwag/pull/3655" />
/// </summary>
public record Client(int ClientId, string Name);

public record ClientMetric(int ClientMetricId, DateOnly RecordedDate, double RecordedValueMetric);

public partial class ClientState : State<ClientState>
{
    public List<Client> Clients { get; private set; } = null!;

    public Client? CurrentClient { get; private set; }

    public List<ClientMetric> CurrentClientMetrics { get; private set; } = null!;

    public ClientMetricChartData? ChartData { get; private set; }

    public override void Initialize()
    {
        Clients = new List<Client>();
        CurrentClientMetrics = new List<ClientMetric>();
    }
}

public class ClientMetricChartData
{
    private readonly Client _client;
    private readonly List<ClientMetric> _metrics;

    public ClientMetricChartData(Client client, List<ClientMetric> metrics)
    {
        _client = client;
        _metrics = metrics;
    }

    public IEnumerable<ClientMetric> Metrics => _metrics.OrderBy(m => m.RecordedDate).Take(12);

    public IEnumerable<string> XAxisLabels => Metrics.Select(m => m.RecordedDate.ToShortDateString());

    public IEnumerable<ChartSeries> Series => new List<ChartSeries>(1)
    {
        new() { Name = _client.Name, Data = Metrics.Select(m => m.RecordedValueMetric).ToArray() }
    };
}
