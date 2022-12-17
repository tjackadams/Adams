using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Nexus.Portal.Infrastructure.Configuration;
using Nexus.Portal.Services;

namespace Nexus.Portal.Pages.Tools;

public partial class GuidGenerator
{
    [Inject]
    public ILocalStorageService Storage { get; set; } = null!;

    [Inject]
    public GuidFormatter Formatter { get; set; } = null!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    public ISnackbar SnackBar { get; set; } = null!;

    private GuidGeneratorSettings _settings = new GuidGeneratorSettings();

    private Guid _value;

    public string? Value { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var settings = await Storage.GetItemAsync<GuidGeneratorSettings>(GuidGeneratorSettings.Key);
            if (settings is not null)
            {
                _settings = settings;
            }

            await GenerateAsync();
        }
    }

    private async Task GenerateAsync()
    {
        _value = Guid.NewGuid();
        await FormatAsync();
    }

    private async Task OnCopy()
    {
        await JSRuntime.InvokeVoidAsync("clipboardCopy.copyText", Value);
        SnackBar.Add("Copied!", Severity.Info, options => options.VisibleStateDuration = 800);
    }

    private async Task FormatAsync()
    {
        Value = await Formatter.FormatAsync(_value, _settings);
        StateHasChanged();
    }
}
