using FractalLab.Common;
using NUnit.Framework;

namespace FractalLab.Common.Tests;

[TestFixture()]
public class MandelbrotSetTests
{
    //
    // Basic test to ensure the calculation works as expected
    // TODO: Add more comprehensive tests including edge cases and performance tests
    //

    [Test()]
    public void CalculateBasicTest()
    {
        var result = MandelbrotSet.CalculateParallel(-2.5, 1.0, -1.0, 1.0, 800, 600, 1000);
        Assert.AreEqual(800, result.GetLength(0));
        Assert.AreEqual(600, result.GetLength(1));
        Assert.AreEqual(1000, result[400, 300]);
        Assert.AreEqual(1, result[0, 0]);
    }

    [Test()]
    public void CalculateParallelTest()
    {
        var result = MandelbrotSet.CalculateParallel(-2.5, 1.0, -1.0, 1.0, 800, 600, 1000);
        Assert.AreEqual(800, result.GetLength(0));
        Assert.AreEqual(600, result.GetLength(1));
        Assert.AreEqual(1000, result[400, 300]);
        Assert.AreEqual(1, result[0, 0]);
    }

    [Test()]
    public async Task CalculateProgressiveTest()
    {
        int?[,] result = await MandelbrotSet.CalculateProgressive(-2.5, 1.0, -1.0, 1.0, 800, 600, 1000, new Progress<int?[,]>(progress =>
        {
            Assert.AreEqual(800, progress.GetLength(0));
            Assert.AreEqual(600, progress.GetLength(1));

        }), default);

        Assert.AreEqual(1000, result[400, 300]);
        Assert.AreEqual(1, result[0, 0]);

        //Ensure all values are filled
        for (int x = 0; x < result.GetLength(0); x++)
        {
            for (int y = 0; y < result.GetLength(1); y++)
            {
                Assert.IsNotNull(result[x, y]);
            }
        }
    }
}