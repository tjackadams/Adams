using Blazored.LocalStorage;
using Nexus.Portal.Infrastructure.Configuration;

namespace Nexus.Portal.Services;

public class GuidFormatter
{
    private readonly ILocalStorageService _storage;

    public GuidFormatter(ILocalStorageService storage)
    {
        _storage = storage;
    }

    public async Task<string> FormatAsync(Guid guid, GuidGeneratorSettings settings)
    {
        var value = guid.ToString(settings.Brackets ? "B" : "D");
        value = settings.Uppercase ? value.ToUpperInvariant() : value.ToLowerInvariant();

        if (!settings.Hyphens)
        {
            value = value.Replace("-", string.Empty);
        }

        await _storage.SetItemAsync(GuidGeneratorSettings.Key, settings);

        return value;
    }
}
