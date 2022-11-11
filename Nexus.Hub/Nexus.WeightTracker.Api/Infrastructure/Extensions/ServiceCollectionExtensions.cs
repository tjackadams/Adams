using Microsoft.Extensions.DependencyInjection.Extensions;
using Nexus.WeightTracker.Api.Infrastructure.Authorization;

namespace Nexus.WeightTracker.Api.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.TryAddScoped<IdentityClaimProvider>();

        return services;
    }
}
