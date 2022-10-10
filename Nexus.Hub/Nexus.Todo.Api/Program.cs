using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Nexus.Todo.Api.Infrastructure;
using Nexus.Todo.Api.Infrastructure.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options.UseInMemoryDatabase("todo");
});

builder.Services.AddMediator(options =>
{
    options.AddConsumers(Assembly.GetExecutingAssembly());
});

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi3();

app.MapTodo();

app.Run();