namespace Nexus.Portal.Services;

public class GuidFormatter
{
    private readonly ClientSettingsManager _settings;
    public GuidFormatter(ClientSettingsManager settings)
    {
        _settings = settings;
    }
    public async Task<string> FormatAsync(Guid guid, bool uppercase, bool brackets, bool hyphens)
    {
        var value = guid.ToString(brackets ? "B" : "D");
        value = uppercase ? value.ToUpperInvariant() : value.ToLowerInvariant();

        if (hyphens)
        {
            value = value.Replace("-", string.Empty);
        }

        var settings = await _settings.GetAsync();
        await _settings.SetAsync(settings with { GuidSettings = new GuidGeneratorOptions(uppercase, brackets, hyphens) });

        return value;
    }
}
