using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.Portal.Services;

namespace Nexus.Portal.Shared;

public partial class MainLayout
{
    [Inject]
    public ClientSettingsManager Settings { get; set; } = null!;

    private bool _drawerOpen;
    private bool _isDarkMode;
    private readonly MudTheme _theme = new MudTheme();

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task DarkModeToggle(bool isDarkMode)
    {
        _isDarkMode = isDarkMode;
        var settings = await Settings.GetAsync();
        await Settings.SetAsync(settings with { IsDarkMode = isDarkMode });
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var settings = await Settings.GetAsync();
            _isDarkMode = settings.IsDarkMode;
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}
