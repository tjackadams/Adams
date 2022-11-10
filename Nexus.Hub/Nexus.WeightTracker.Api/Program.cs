using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.AspNetCore.Behaviours;
using Nexus.WeightTracker.Api.Domain;
using Nexus.WeightTracker.Api.Infrastructure;
using Nexus.WeightTracker.Api.Infrastructure.Endpoints;
using Nexus.WeightTracker.Api.Infrastructure.NSwag;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(options =>
{
    options.Title = "Nexus Weight Tracking API";

    options.SchemaNameGenerator = new NexusSchemaNameGenerator();

    options.TypeMappers.Add(new PrimitiveTypeMapper(typeof(ClientId), schema =>
    {
        schema.Type = JsonObjectType.Integer;
    }));
});

builder.Services.AddDbContext<WeightDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("Nexus"));
});

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehaviour<,>));


AssemblyScanner.FindValidatorsInAssembly(typeof(Program).Assembly)
    .ForEach(item => builder.Services.AddScoped(typeof(IValidator), item.ValidatorType));

var app = builder.Build();

app.UseHttpLogging();

app.UseOpenApi();
app.UseSwaggerUi3();

app.MapClient();

app.Run();
