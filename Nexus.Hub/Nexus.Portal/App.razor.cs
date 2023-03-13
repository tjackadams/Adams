using BlazorState.Features.JavaScriptInterop;
using BlazorState.Features.Routing;
using BlazorState.Pipeline.ReduxDevTools;
using Microsoft.AspNetCore.Components;

namespace Nexus.Portal;

public partial class App : ComponentBase
{
    private JsonRequestHandler? _jsonRequestHandler;
    private ReduxDevToolsInterop? _reduxDevToolsInterop;
    private RouteManager? _routeManager;

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = null!;

    protected override void OnInitialized()
    {
        _jsonRequestHandler = ServiceProvider.GetService<JsonRequestHandler>();
        _reduxDevToolsInterop = ServiceProvider.GetService<ReduxDevToolsInterop>();
        _routeManager = ServiceProvider.GetService<RouteManager>();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_reduxDevToolsInterop is not null)
        {
            await _reduxDevToolsInterop.InitAsync();
        }

        if (_jsonRequestHandler is not null)
        {
            await _jsonRequestHandler.InitAsync();
        }
    }
}
