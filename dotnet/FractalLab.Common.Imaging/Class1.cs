using SkiaSharp;

namespace FractalLab.Common.Imaging;

public static class ImageGenerator
{
    public static void PrintResultImage(int[,] result, string saveDirectory)
    {
        int width = result.GetLength(0);
        int height = result.GetLength(1);
        using var bitmap = new SKBitmap(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int value = result[x, y];
                byte colorValue = (byte)(255 * value / 1000.0);
                colorValue = (byte)Math.Clamp((int)colorValue, 0, 255);
                var color = new SKColor(colorValue, colorValue, colorValue);
                bitmap.SetPixel(x, y, color);
            }
        }
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        string fileName = Path.Combine(saveDirectory,  $"mandelbrot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
        using var stream = File.OpenWrite(fileName);
        data.SaveTo(stream);
        Console.WriteLine($"Image saved as {fileName}");
    }
}
