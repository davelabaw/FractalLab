namespace FractalLab.Common;

public static class MandelbrotSet
{
    public static int[,] CalculateBasic(double xMin, double xMax, double yMin, double yMax, int width, int height, int maxIterations)
    {
        int[,] mandelbrotSet = new int[width, height];
        for (int px = 0; px < width; px++)
        {
            for (int py = 0; py < height; py++)
            {
                double x0 = xMin + (px / (double)width) * (xMax - xMin);
                double y0 = yMin + (py / (double)height) * (yMax - yMin);
                double x = 0;
                double y = 0;
                int iteration = 0;
                while (x * x + y * y <= 4 && iteration < maxIterations)
                {
                    double xTemp = x * x - y * y + x0;
                    y = 2 * x * y + y0;
                    x = xTemp;
                    iteration++;
                }
                mandelbrotSet[px, py] = iteration;
            }
        }
        return mandelbrotSet;
    }

    public static int[,] CalculateParallel(double xMin, double xMax, double yMin, double yMax, int width, int height, int maxIterations)
    {
        int[,] mandelbrotSet = new int[width, height];
        Parallel.For(0, width, px =>
        {
            for (int py = 0; py < height; py++)
            {
                double x0 = xMin + (px / (double)width) * (xMax - xMin);
                double y0 = yMin + (py / (double)height) * (yMax - yMin);
                mandelbrotSet[px, py] = GetIterationCount(x0, y0, maxIterations);
            }
        });
        return mandelbrotSet;
    }

    public static async Task<int[,]> CalculateParallelAsync(double xMin, double xMax, double yMin, double yMax, int width, int height, int maxIterations, CancellationToken cancellationToken)
    {
        return await Task.Run(() => CalculateParallel(xMin, xMax, yMin, yMax, width, height, maxIterations), cancellationToken);
    }

    public static async Task<int?[,]> CalculateProgressive(double xMin, double xMax, double yMin, double yMax, int width, int height, int maxIterations, IProgress<int?[,]> progress, CancellationToken cancellationToken)
    {
        int?[,] mandelbrotSet = new int?[width, height];
        for (int px = 0; px < width; px++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            for (int py = 0; py < height; py++)
            {
                double x0 = xMin + (px / (double)width) * (xMax - xMin);
                double y0 = yMin + (py / (double)height) * (yMax - yMin);
                mandelbrotSet[px, py] = GetIterationCount(x0, y0, maxIterations);
            }
            progress.Report(mandelbrotSet);
            await Task.Yield(); // Yield to keep UI responsive
        }
        return mandelbrotSet;
    }

    public static int GetIterationCount(double x0, double y0, int maxIterations)
    {
        double x = 0;
        double y = 0;
        int iteration = 0;
        while (x * x + y * y < 4 && iteration < maxIterations)
        {
            double xTemp = x * x - y * y + x0;
            y = 2 * x * y + y0;
            x = xTemp;
            iteration++;
        }
        return iteration;
    }

    public static bool IsInMandelbrotSet(double x0, double y0, int maxIterations)
    {
        return GetIterationCount(x0, y0, maxIterations) == maxIterations;
    }

    public static (double xMin, double xMax, double yMin, double yMax) Zoom(double centerX, double centerY, double scale, double xMin, double xMax, double yMin, double yMax)
    {
        double width = xMax - xMin;
        double height = yMax - yMin;
        double newWidth = width / scale;
        double newHeight = height / scale;
        double newXMin = centerX - newWidth / 2;
        double newXMax = centerX + newWidth / 2;
        double newYMin = centerY - newHeight / 2;
        double newYMax = centerY + newHeight / 2;
        return (newXMin, newXMax, newYMin, newYMax);
    }
}
