using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.Portal.Features.Clients;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class ClientSelection
{
    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    private ClientState ClientState => GetState<ClientState>();

    protected override async Task OnInitializedAsync()
    {
        await Mediator.Send(new ClientState.GetAllClientsAction());
    }

    private async Task OnClientChanged(Client client)
    {
        await Mediator.Send(new ClientState.SetCurrentClientAction(client));
    }

    private async void OpenDialog()
    {
        var dialog = await DialogService.ShowAsync<AddClientDialog>("Add Client");
        _ = await dialog.Result;
    }
}
