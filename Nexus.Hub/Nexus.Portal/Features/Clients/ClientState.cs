using BlazorState;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Features.Clients;

/// <summary>
///     Client is used in a select list so it needs equality operators. Currently the ClientViewModel that
///     comes from the api is a class, so we map this to a record as a workaround.
///     <see href="https://github.com/RicoSuter/NSwag/pull/3655" />
/// </summary>
public record Client(int ClientId, string Name);

public partial class ClientState : State<ClientState>
{
    public List<Client> Clients { get; private set; } = null!;

    public Client? CurrentClient { get; private set; }

    public List<ClientMetricViewModel> CurrentClientMetrics { get; private set; } = null!;

    public override void Initialize()
    {
        Clients = new List<Client>();
        CurrentClientMetrics = new List<ClientMetricViewModel>();
    }
}
