using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace Nexus.WeightTracker.Api.Infrastructure.Extensions;

public static class AuthenticationExtensions
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Reader", policy => policy.RequireRole("WeightTracker.Read"));
            options.AddPolicy("Writer", policy => policy.RequireRole("WeightTracker.Write"));
        });

        return builder;
    }
}
