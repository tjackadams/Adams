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
    private List<ClientViewModel> _clients = new();

    private bool _shouldRender;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    [Inject]
    public IMediator Mediator { get; set; } = null!;

    [Parameter]
    public EventCallback<ClientViewModel> ClientChanged { get; set; }

    [Parameter]
    public List<ClientViewModel> Clients { get; set; } = null!;


    private async Task OnClientChanged(ClientViewModel client)
    {
        await Mediator.Send(new ClientState.SetCurrentClientAction(client));
    }

    private async void OpenDialog()
    {
        var dialog = await DialogService.ShowAsync<AddClientDialog>("Add Client");
        _ = await dialog.Result;
    }

    protected override void OnParametersSet()
    {
        _shouldRender = Clients != _clients;
        _clients = Clients;
    }

    protected override bool ShouldRender()
    {
        return _shouldRender;
    }
}
