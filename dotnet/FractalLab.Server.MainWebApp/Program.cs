using FractalLab.Common;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    //options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      //builder =>
                      //{
                      //    builder.WithOrigins("http://example.com",
                      //                        "http://localhost:5173/")
                      //           .AllowAnyHeader()
                      //           .AllowAnyMethod();
                      //});
    builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
});

var app = builder.Build();

//app.UseCors("MyAllowSpecificOrigins");


//var sampleTodos = new Todo[] {
//    new(1, "Walk the dog"),
//    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
//    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
//    new(4, "Clean the bathroom"),
//    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
//};

var rootApi = app.MapGroup("/");
rootApi.MapGet("/", () => "Fractal Lab API is running. Use the /todos or /fractal endpoints.");

//var todosApi = app.MapGroup("/todos");
//todosApi.MapGet("/", () => sampleTodos);


var fractalApi = app.MapGroup("/fractal");
fractalApi.MapGet("/", () => "Fractal API is running. Use the /mandelbrot-set endpoint to generate fractal data.");
//fractalApi.MapGet("/mandelbrot-set/", () => "Use the /mandelbrot-set/{xMin}/{xMax}/{yMin}/{yMax}/{width}/{height}/{maxIterations}/{provider?} endpoint to generate fractal data. Example: /mandelbrot-set/-2.5/1.0/-1.0/1.0/800/600/1000");
fractalApi.MapGet("/mandelbrot-set/{xMin}/{xMax}/{yMin}/{yMax}/{width}/{height}/{maxIterations}/{provider?}", (double xMin, double xMax, double yMin, double yMax, int width, int height, int maxIterations, string? provider = default) =>
{
    //TODO: Validate parameters
    //TODO: Support different providers

    int[,] result = MandelbrotSet.CalculateParallel(xMin, xMax, yMin, yMax, width, height, maxIterations);


    List<List<int>> resultList = new List<List<int>>(width);
    for (int x = 0; x < width; x++)
    {
        List<int> column = new List<int>(height);
        for (int y = 0; y < height; y++)
        {
            column.Add(result[x, y]);
        }
        resultList.Add(column);
    }
    var resultJson = JsonSerializer.Serialize(resultList);

    //var resultJson = System.Text.Json.JsonSerializer.Serialize(result);
    MandelbrotSetResponse? mandelbrotSetResponse = new MandelbrotSetResponse(resultJson);
    return (mandelbrotSetResponse != null)
        ? Results.Ok(mandelbrotSetResponse)
        : Results.NotFound();
});

app.Run();

//public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

//[JsonSerializable(typeof(Todo[]))]
//internal partial class AppJsonSerializerContext : JsonSerializerContext
//{

//}


public record MandelbrotSetResponse(string resultJson);


[JsonSerializable(typeof(MandelbrotSetResponse[]))]
[JsonSerializable(typeof(JsonElement))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}