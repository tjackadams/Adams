using Blazored.LocalStorage;

namespace Nexus.Portal.Services;

public record ClientSetting(bool IsDarkMode, GuidGeneratorOptions? GuidSettings);
public record GuidGeneratorOptions(bool Uppercase, bool Brackets, bool Hyphens);
public class ClientSettingsManager
{
    private readonly ILocalStorageService _storage;
    public ClientSettingsManager(ILocalStorageService storage)
    {
        _storage = storage;
    }

    public async Task SetAsync(ClientSetting setting)
    {
        await _storage.SetItemAsync("Settings", setting);
    }

    public async Task<ClientSetting> GetAsync()
    {
        var settings =  await _storage.GetItemAsync<ClientSetting>("Setting");
        if(settings == null)
        {
            return new ClientSetting(false, null);
        }

        return settings;
    }
}
