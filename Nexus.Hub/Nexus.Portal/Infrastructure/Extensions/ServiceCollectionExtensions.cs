using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Nexus.Portal.Infrastructure.Http;
using Nexus.Portal.Infrastructure.Polly;
using Nexus.Portal.Services;
using Nexus.Todo;
using Nexus.WeightTracker;

namespace Nexus.Portal.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<GuidFormatter>();

        services.AddTransient<LoggerProviderMessageHandler<TodoClient>>();
        services.AddTransient<LoggerProviderMessageHandler<WeightTrackerClient>>();
        services.AddTransient<ShowOperationProgressMessageHandler>();

        services.AddHttpClient<TodoClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>();
            client.BaseAddress = new Uri(settings.Value.ApiGatewayUri, "todo/");
        })
            .AddHttpMessageHandler<ShowOperationProgressMessageHandler>()
            .AddHttpMessageHandler<LoggerProviderMessageHandler<TodoClient>>()
            .AddDefaultRetryPolicy();

        services.AddHttpClient<WeightTrackerClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>();
            client.BaseAddress = new Uri(settings.Value.ApiGatewayUri, "weighttracker/");
        })
            .AddHttpMessageHandler<LoggerProviderMessageHandler<WeightTrackerClient>>()
            .AddDefaultRetryPolicy()
            .AddMicrosoftIdentityUserAuthenticationHandler(nameof(WeightTrackerClient), options =>
            {
                options.Scopes = "api://cef30c8d-dc02-4e0f-aa61-52155ec9a9a6/nexus.weighttracker";
            })
            .AddHttpMessageHandler<ShowOperationProgressMessageHandler>();

        return services;
    }
}
