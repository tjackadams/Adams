using Microsoft.Extensions.Options;
using Nexus.Portal.Infrastructure.Polly;
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

        services.AddTransient<LoggerProviderMessageHandler<Client>>();
        services.AddTransient<LoggerProviderMessageHandler<WeightTrackerClient>>();

        services.AddHttpClient<Client>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>();
            client.BaseAddress = new Uri(settings.Value.ApiGatewayUri, "todo/");
        })
            .AddHttpMessageHandler<LoggerProviderMessageHandler<Client>>()
            .AddDefaultRetryPolicy();

        services.AddHttpClient<WeightTrackerClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>();
            client.BaseAddress = new Uri(settings.Value.ApiGatewayUri, "weighttracker/");
        })
            .AddHttpMessageHandler<LoggerProviderMessageHandler<WeightTrackerClient>>()
            .AddDefaultRetryPolicy();

        return services;
    }
}
