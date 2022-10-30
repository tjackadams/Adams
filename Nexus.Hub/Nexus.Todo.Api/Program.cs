using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.Todo.Api.Infrastructure;
using Nexus.Todo.Api.Infrastructure.Endpoints;
using Nexus.Todo.Api.Infrastructure.NSwag;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(options =>
{
    options.Title = "Nexus Todo API";

    options.SchemaNameGenerator = new NexusSchemaNameGenerator();
});

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("Nexus"));
});

builder.Services.AddMediatR(typeof(Program));
AssemblyScanner.FindValidatorsInAssembly(typeof(Program).Assembly)
    .ForEach(item => builder.Services.AddScoped(typeof(IValidator), item.ValidatorType));

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi3();

app.MapTodo();

app.Run();

public partial class Program { }


