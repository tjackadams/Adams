using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.Portal.Features.Clients;
using Nexus.Portal.Features.GlobalProgress;

namespace Nexus.Portal.Components;

public partial class ClientSelection
{
    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    [Inject]
    public NavigationManager NavManager { get; set; } = null!;

    [Parameter]
    public int? ClientId { get; set; }

    private ClientState ClientState => GetState<ClientState>();

    private List<Client> Clients => ClientState.Clients;

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

    private void OnClientChanged(Client client)
    {
        NavManager.NavigateTo($"/tracker/weight/{client.ClientId}");
    }

    private async void OpenDialog()
    {
        var dialog = await DialogService.ShowAsync<AddClientDialog>("Add Client");
        _ = await dialog.Result;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ClientId is not null)
        {
            if (Clients.Any())
            {
                var client = Clients.Where(c => c.ClientId == ClientId).FirstOrDefault();
                if (client is not null)
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
            }
        }
    }
}
