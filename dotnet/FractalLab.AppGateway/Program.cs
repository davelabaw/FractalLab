using FractalLab.Common;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//This can be a public API for now. Later we may restrict it to specific origins.
builder.Services.AddCors(builder =>
{
    builder.AddPolicy(name: "MyAllowSpecificOrigins",
        builder => builder
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("MyAllowSpecificOrigins");

#region Fractal API members
//TODO: Move to separate file

//TOOD: Add health checks
//TODO: Add logging
var fractalApi = app.MapGroup("/fractal");
fractalApi.MapGet("/", () => "Fractal API is running. Use the /mandelbrot-set endpoint to generate fractal data.");
fractalApi.MapGet("/mandelbrot-set/{xMin}/{xMax}/{yMin}/{yMax}/{width}/{height}/{maxIterations}/{provider?}", (double xMin, double xMax, double yMin, double yMax, int width, int height, int maxIterations, string? provider = default) =>
{
    var requestedAt = DateTime.UtcNow;

    //TODO: Validate parameters
    //TODO: Support different providers

    int[,] result = MandelbrotSet.CalculateParallel(xMin, xMax, yMin, yMax, width, height, maxIterations);

    /*
     PROBLEM: Apparently System.Text.Json cannot serialize 2D arrays directly.
        + Temporary solution: Break it down to a list of rows, serialize each row, then serialize the list of rows.
        + A better solution: Create a custom JsonConverter for 2D arrays. (Or revaluate the problem...)
     */

    List<string> resultList = new List<string>(width);
    for (int y = 0; y < width; y++)
    {
        List<int> columns = new List<int>(height);
        for (int x = 0; x < height; x++)
        {
            columns.Add(result[y, x]);
        }
        var rowJson = JsonSerializer.Serialize(columns);
        resultList.Add(rowJson);
    }
    var resultJson = JsonSerializer.Serialize(resultList);

    MandelbrotSetResponse? mandelbrotSetResponse = new MandelbrotSetResponse(
        resultJson, xMin, xMax, yMin, yMax, width, height, maxIterations, provider ?? "default", requestedAt, DateTime.UtcNow
    );
    return (mandelbrotSetResponse != null)
        ? Results.Ok(mandelbrotSetResponse)
        : Results.NotFound();
});
#endregion

app.Run();

//TODO: Move...
internal record MandelbrotSetResponse (
    string rowsJson,

    //Echo back the parameters for reference
    double xMin, double xMax, double yMin, double yMax, int width, int height, int maxIterations,

    string provider, 
    DateTime requestedAt, DateTime fininishedAt
);