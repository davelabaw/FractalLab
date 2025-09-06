using FractalLab.Common;
using FractalLab.Common.Imaging;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.ResetColor();
        Console.Clear();

        Console.Write("Welcome to the fun world of fractals! Maximize your browsers and maybe zoom out your console window's text. Press Enter to start!");
        Console.ReadLine(); //Keep console open

        // Get number of iterations from user
        Console.Write("\r\nEnter the number of loops (e.g., 5): ");
        int loops = int.Parse(Console.ReadLine() ?? "5");

        Console.Write($"\r\nRendering Mandelbrot with {loops}x increasing clarity...");

        double xMin = -2.5;
        double xMax = 1.0;
        double yMin = -1.0;
        double yMax = 1.0;
        int width = 256;
        int height = 128;
        int iteration = 150;

        Console.Clear();
        for (int x = 0; x < loops; x++)
        {
            //Start at top of the console
            Console.SetCursorPosition(0, 0);
            
            Console.WriteLine($"{x} of {loops}");

            if (x > 0)
            {
                iteration = (800 * x);

                if (x == 1)
                {
                    // Initial zoom center 
                    var newCoordinates = MandelbrotSet.Zoom(0.27500402040511, 0.0061520017195, 1.75, xMin, xMax, yMin, yMax);
                    xMin = newCoordinates.xMin;
                    xMax = newCoordinates.xMax;
                    yMin = newCoordinates.yMin;
                    yMax = newCoordinates.yMax;
                }
                else
                {
                    var newCoordinates = MandelbrotSet.Zoom((xMin + xMax) / 2, (yMin + yMax) / 2, 1.75, xMin, xMax, yMin, yMax);
                    xMin = newCoordinates.xMin;
                    xMax = newCoordinates.xMax;
                    yMin = newCoordinates.yMin;
                    yMax = newCoordinates.yMax;
                }
            }
            int[,] result = MandelbrotSet.CalculateParallel(xMin, xMax, yMin, yMax, width, height, iteration);
            
            //PrintResultText(result);
            PrintColor(result);
            //DownloadImage(result);

            Console.WriteLine();
        }
        
        Console.WriteLine("Rendering complete! Press Enter to exit.");
        Console.ReadLine(); //Keep console open
    }

    private static void PrintResultText(int[,] result)
    {
        int width = result.GetLength(0);
        int height = result.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int value = result[x, y];
                char c = value == 1500 ? '#' : value > 1000 ? 'O' : value > 500 ? '+' : value > 100 ? '.' : ' ';
                Console.Write(c);
            }
            Console.WriteLine();
        }
    }

    static void PrintColor(int[,] result)
    {
        int width = result.GetLength(0);
        int height = result.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var val = result[x, y];

                //Map the value to console colors after bucketing it into 16 segments (0-15)
                var mappedVal = (val * 16 / 512);
                if (mappedVal >= 15)
                    mappedVal = 0; //White
                else if (mappedVal <= 0)
                    mappedVal = 1; 

                ////Map the value to a byte (0-255)
                //byte byteVal = (byte)((val % 255));
                //var mappedVal = (byteVal * 16 / 255);

                //if(mappedVal == 15)
                //    mappedVal = 0; //Black


                //Map the value to a console color (0-15)
                //var mappedVal = (val % 16);

                Console.BackgroundColor = (ConsoleColor)mappedVal;
                Console.Write("  "); 
            }
            Console.ResetColor();
            Console.Write("\r\n");
        }
    }

    private static void DownloadImage(int[,] result)
    {
        var saveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FractalLab");

        ImageGenerator.PrintResultImage(result, saveDirectory);
    }
}