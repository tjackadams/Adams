using BlazorState.Features.JavaScriptInterop;
using BlazorState.Features.Routing;
using BlazorState.Pipeline.ReduxDevTools;
using Microsoft.AspNetCore.Components;

namespace Nexus.Portal;

public partial class App : ComponentBase
{
    [Inject]
    private JsonRequestHandler JsonRequestHandler { get; set; } = null!;

    [Inject]
    private ReduxDevToolsInterop ReduxDevToolsInterop { get; set; } = null!;

    [Inject]
    private RouteManager RouteManager { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await ReduxDevToolsInterop.InitAsync();
        await JsonRequestHandler.InitAsync();
    }
}
