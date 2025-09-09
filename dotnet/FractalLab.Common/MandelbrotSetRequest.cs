namespace FractalLab.Common;

public record MandelbrotSetRequest(
    double XMin,
    double XMax,
    double YMin,
    double YMax,
    int Width,
    int Height,
    int MaxIterations,
    string? Provider = default
);
