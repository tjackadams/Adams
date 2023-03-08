using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using MudBlazor;
using Nexus.Portal.Features.Clients;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientSelection
{
    [Inject]
    public WeightTrackerClient TrackerClient { get; set; } = null!;

    [Inject]
    public MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = null!;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    [Inject]
    public IMediator Mediator { get; set; } = null!;

    [Parameter]
    public EventCallback<GetClientList_ClientModel> ClientChanged { get; set; }

    [CascadingParameter]
    public ClientStateProvider? ClientStateProvider { get; set; }

    [Parameter]
    public List<GetClientList_ClientModel> Clients { get; set; } = null!;

    private List<GetClientList_ClientModel> _clients = new ();


    private async Task OnClientChanged(GetClientList_ClientModel client)
    {
        await Mediator.Send(new ClientState.SetCurrentClientAction(client));
    }

    private async void OpenDialog()
    {
        var dialog = await DialogService.ShowAsync<AddClientDialog>("Add Client");
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            try
            {
                ClientStateProvider?.GetClientsAsync();
            }
            catch (Exception ex)
            {
                ConsentHandler?.HandleException(ex);
            }
        }
    }

    private bool _shouldRender;

    protected override void OnParametersSet()
    {
        _shouldRender = Clients != _clients;
        _clients = Clients;
    }

    protected override bool ShouldRender() => _shouldRender;
}
