using BlazorState;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Features.Clients;

public partial class ClientState : State<ClientState>
{
    public List<ClientViewModel> Clients { get; private set; } = null!;

    public ClientViewModel? CurrentClient { get; private set; }

    public override void Initialize()
    {
        Clients = new List<ClientViewModel>();
    }
}
