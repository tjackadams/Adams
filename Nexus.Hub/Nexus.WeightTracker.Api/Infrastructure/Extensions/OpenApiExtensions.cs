using Microsoft.AspNetCore.Authentication.JwtBearer;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure.NSwag;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Nexus.WeightTracker.Api.Infrastructure.Extensions;

public static class OpenApiExtensions
{
    public static WebApplicationBuilder AddOpenApi(this WebApplicationBuilder builder)
    {
        var swagger = builder.Configuration.GetRequiredSection("Swagger");

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOpenApiDocument(document =>
        {
            document.Title = "Nexus Weight Tracking API";

            document.SchemaSettings.SchemaNameGenerator = new NexusSchemaNameGenerator();

            document.SchemaSettings.TypeMappers.Add(new PrimitiveTypeMapper(typeof(ClientId), schema =>
            {
                schema.Type = JsonObjectType.Integer;
            }));

            document.SchemaSettings.TypeMappers.Add(new PrimitiveTypeMapper(typeof(ClientMetricId), schema =>
            {
                schema.Type = JsonObjectType.Integer;
            }));

            document.AddSecurity(JwtBearerDefaults.AuthenticationScheme, Enumerable.Empty<string>(),
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flow = OpenApiOAuth2Flow.AccessCode,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = $"{swagger.GetValue<string>("Instance")}{swagger.GetValue<string>("TenantId")}/oauth2/v2.0/authorize",
                            TokenUrl = $"{swagger.GetValue<string>("Instance")}{swagger.GetValue<string>("TenantId")}/oauth2/v2.0/token",
                            Scopes = { { "api://cef30c8d-dc02-4e0f-aa61-52155ec9a9a6/nexus.weighttracker", "Nexus.WeightTracker" } }
                        }
                    }
                });

            document.OperationProcessors.Add(
                new AspNetCoreOperationSecurityScopeProcessor(JwtBearerDefaults.AuthenticationScheme)
            );
        });

        return builder;
    }
}
