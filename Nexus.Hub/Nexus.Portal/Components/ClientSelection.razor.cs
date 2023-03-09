using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.Portal.Features.Clients;
using Nexus.Portal.Features.GlobalProgress;

namespace Nexus.Portal.Components;

public partial class ClientSelection
{
    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    private ClientState ClientState => GetState<ClientState>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await Mediator.Send(new ProgressState.ShowProgressAction());
            await Mediator.Send(new ClientState.GetAllClientsAction());
        }
        finally
        {
            await Mediator.Send(new ProgressState.HideProgressAction());
        }
    }

    private async Task OnClientChanged(Client client)
    {
        try
        {
            await Mediator.Send(new ProgressState.ShowProgressAction());
            await Mediator.Send(new ClientState.SetCurrentClientAction(client));
        }
        finally
        {
            await Mediator.Send(new ProgressState.HideProgressAction());
        }
    }

    private async void OpenDialog()
    {
        var dialog = await DialogService.ShowAsync<AddClientDialog>("Add Client");
        _ = await dialog.Result;
    }
}
