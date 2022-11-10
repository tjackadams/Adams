using Microsoft.Extensions.Options;
using Nexus.Portal.Services;
using Nexus.Todo;
using Nexus.WeightTracker;

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
            client.BaseAddress = new Uri(settings.Value.ApiGatewayUri, "todo/");
        });

        services.AddHttpClient<WeightTrackerClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>();
            client.BaseAddress = new Uri(settings.Value.ApiGatewayUri, "weighttracker/");
        });

        return services;
    }
}
