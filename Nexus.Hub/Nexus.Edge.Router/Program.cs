var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Services.AddLettuceEncrypt();
}

builder.Services.AddReverseProxy()
.LoadFromConfig(builder.Configuration.GetRequiredSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();

app.Run();
