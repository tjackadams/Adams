using Microsoft.Extensions.Options;
using Nexus.Portal;
using Nexus.Todo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<Settings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<Client>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<Settings>>();
    client.BaseAddress = settings.Value.ApiGatewayUrl;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
