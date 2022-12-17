using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.Portal.Infrastructure.Configuration;

namespace Nexus.Portal.Shared;

public partial class MainLayout
{
    [Inject]
    public ILocalStorageService Storage { get; set; } = null!;

    private bool _drawerOpen;
    private bool _isReady;
    private UserPreference? _userPreference;
    private readonly MudTheme _theme = new ();

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task DarkModeToggle()
    {
        await Storage.SetItemAsync(UserPreference.Key, _userPreference);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var userPreference = await Storage.GetItemAsync<UserPreference>(UserPreference.Key);
            _userPreference = userPreference ?? new UserPreference();
            _isReady = true;
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}
