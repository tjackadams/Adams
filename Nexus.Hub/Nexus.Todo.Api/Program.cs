﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nexus.AspNetCore.Behaviours;
using Nexus.Todo.Api.Infrastructure;
using Nexus.Todo.Api.Infrastructure.Endpoints;
using Nexus.Todo.Api.Infrastructure.NSwag;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(options =>
{
    options.Title = "Nexus Todo API";
    options.SchemaSettings.SchemaNameGenerator = new NexusSchemaNameGenerator();
});

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("Nexus"));
});

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(Program).Assembly);
    options.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
});

AssemblyScanner.FindValidatorsInAssembly(typeof(Program).Assembly)
    .ForEach(item => builder.Services.AddScoped(typeof(IValidator), item.ValidatorType));

var app = builder.Build();

app.UseHttpLogging();

app.UseOpenApi();
app.UseSwaggerUi();

app.MapTodo();

app.Run();

public partial class Program { }


