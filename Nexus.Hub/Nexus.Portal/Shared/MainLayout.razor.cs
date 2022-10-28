namespace Nexus.Portal.Shared;

public partial class MainLayout
{
    private bool _drawerOpen;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}
