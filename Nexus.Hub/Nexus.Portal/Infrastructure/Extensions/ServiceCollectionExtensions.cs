using Microsoft.Extensions.Options;
using Nexus.Portal.Services;
using Nexus.Todo;

namespace Nexus.Portal.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ClientSettingsManager>();

        services.AddScoped<GuidFormatter>();

        services.AddHttpClient<Client>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>();
            client.BaseAddress = settings.Value.ApiGatewayUrl;
        });

        return services;
    }
}
