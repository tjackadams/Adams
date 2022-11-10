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

        services.AddTransient<LoggerProviderMessageHandler<TodoClient>>();
        services.AddTransient<LoggerProviderMessageHandler<WeightTrackerClient>>();

        services.AddHttpClient<TodoClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>();
            client.BaseAddress = new Uri(settings.Value.ApiGatewayUri, "todo/");
        })
            .AddHttpMessageHandler<LoggerProviderMessageHandler<TodoClient>>()
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
