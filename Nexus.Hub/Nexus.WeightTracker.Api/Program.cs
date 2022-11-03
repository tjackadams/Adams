using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.WeightTracker.Api.Infrastructure;
using Nexus.WeightTracker.Api.Infrastructure.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WeightDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("Nexus"));
});

builder.Services.AddMediatR(typeof(Program));
AssemblyScanner.FindValidatorsInAssembly(typeof(Program).Assembly)
    .ForEach(item => builder.Services.AddScoped(typeof(IValidator), item.ValidatorType));

var app = builder.Build();

app.MapClient();

app.Run();
