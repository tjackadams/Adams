using BlazorState;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Features.Clients;

public partial class ClientState : State<ClientState>
{
    public List<GetClientList_ClientModel> Clients { get; private set; } = null!;

    public GetClientList_ClientModel? CurrentClient { get; private set; }

    public override void Initialize()
    {
        Clients = new List<GetClientList_ClientModel>();
    }
}
