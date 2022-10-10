var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi3();

app.MapGet("/weatherforecasts", () =>
{
    var Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var startDate = DateTime.Now;

    return Results.Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = startDate.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    }).ToArray());
}).Produces<IEnumerable<WeatherForecast>>();

app.Run();


public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}