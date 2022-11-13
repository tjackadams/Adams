using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Nexus.AspNetCore.Behaviours;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure;
using Nexus.WeightTracker.Api.Infrastructure.Endpoints;
using Nexus.WeightTracker.Api.Infrastructure.ErrorHandling;
using Nexus.WeightTracker.Api.Infrastructure.Extensions;
using Nexus.WeightTracker.Api.Infrastructure.NSwag;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var swagger = builder.Configuration.GetRequiredSection("Swagger");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Reader", policy => policy.RequireRole("WeightTracker.Read"));
    options.AddPolicy("Writer", policy => policy.RequireRole("WeightTracker.Write"));
});

builder.AddOpenTelemetry();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Nexus Weight Tracking API";

    document.SchemaNameGenerator = new NexusSchemaNameGenerator();

    document.TypeMappers.Add(new PrimitiveTypeMapper(typeof(ClientId), schema =>
    {
        schema.Type = JsonObjectType.Integer;
    }));

    document.TypeMappers.Add(new PrimitiveTypeMapper(typeof(ClientMetricId), schema =>
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

builder.Services.AddDbContext<WeightDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("Nexus"), x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Weight"));
});

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehaviour<,>));


AssemblyScanner.FindValidatorsInAssembly(typeof(Program).Assembly)
    .ForEach(item => builder.Services.AddScoped(typeof(IValidator), item.ValidatorType));

builder.Services.AddServices();

var app = builder.Build();

app.UseHttpLogging();

app.UseErrorHandling();

app.UseOpenApi();
app.UseSwaggerUi3(settings =>
{
    settings.OAuth2Client = new OAuth2ClientSettings
    {
        AppName = "Nexus WeightTracker API - Swagger UI",
        ClientId = swagger.GetValue<string>("ClientId"),
        ClientSecret = string.Empty,
        UsePkceWithAuthorizationCodeGrant = true,
        Scopes = { "api://cef30c8d-dc02-4e0f-aa61-52155ec9a9a6/nexus.weighttracker" }
    };
});

app.UseAuthentication();
app.UseAuthorization();

app.MapClient();

app.Run();
