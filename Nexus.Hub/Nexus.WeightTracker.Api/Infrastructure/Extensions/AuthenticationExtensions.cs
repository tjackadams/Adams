using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Nexus.WeightTracker.Api.Infrastructure.Authorization;

namespace Nexus.WeightTracker.Api.Infrastructure.Extensions;

public static class AuthenticationExtensions
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicyNames.Reader, policy => policy.RequireRole("WeightTracker.Read"));
            options.AddPolicy(AuthorizationPolicyNames.Writer, policy => policy.RequireRole("WeightTracker.Write"));
        });

        return builder;
    }
}
