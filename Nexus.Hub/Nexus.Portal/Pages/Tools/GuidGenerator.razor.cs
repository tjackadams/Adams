using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Nexus.Portal.Services;

namespace Nexus.Portal.Pages.Tools;

public partial class GuidGenerator
{
    [Inject]
    public ClientSettingsManager Settings { get; set; } = null!;

    [Inject]
    public GuidFormatter Formatter { get; set; } = null!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    public bool Uppercase { get; set; }

    public bool Brackets { get; set; }

    public bool Hyphens { get; set; }

    private Guid _value;

    public string? Value { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var settings = await Settings.GetAsync();
        if (settings.GuidSettings is not null)
        {
            Uppercase = settings.GuidSettings.Uppercase;
            Brackets = settings.GuidSettings.Brackets;
            Hyphens = settings.GuidSettings.Hyphens;
        }

        await GenerateAsync();

        await base.OnInitializedAsync();
    }

    private async Task GenerateAsync()
    {
        _value = Guid.NewGuid();
        await FormatAsync();
    }

    private async Task OnUppercaseChange(bool uppercase)
    {
        Uppercase = uppercase;

        await FormatAsync();
    }

    private async Task OnBracketsChange(bool brackets)
    {
        Brackets = brackets;

        await FormatAsync();
    }

    private async Task OnHyphenChange(bool hyphens)
    {
        Hyphens = hyphens;

        await FormatAsync();
    }

    private async Task OnCopy()
    {
        await JSRuntime.InvokeVoidAsync("clipboardCopy.copyText", Value);
    }

    private async Task FormatAsync()
    {
        Value = await Formatter.FormatAsync(_value, Uppercase, Brackets, Hyphens);
    }
}
