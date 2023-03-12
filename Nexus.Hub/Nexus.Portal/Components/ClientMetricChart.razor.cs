using Nexus.Portal.Features.Clients;

namespace Nexus.Portal.Components;

public partial class ClientMetricChart
{
    private int Index = -1;

    private ClientState ClientState => GetState<ClientState>();
    private Client? CurrentClient => ClientState.CurrentClient;

    private ClientMetricChartData? ChartData => ClientState.ChartData;

}
