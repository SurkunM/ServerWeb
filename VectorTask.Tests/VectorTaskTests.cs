namespace VectorTask.Tests;

public class VectorTaskTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(new double[] { })]
    public void ConstructorExceptionsTest(double[]? components)
    {
        Assert.ThrowsAny<Exception>(() => new Vector(components!));
    }

    [Theory]
    [InlineData(new double[] { 1, 2, 3 })]
    [InlineData(new double[] { 0 })]
    [InlineData(new double[] { -1, -2, -3 })]
    public void ConstructorNotExceptionTest(double[] components)
    {
        var exception = Record.Exception(() => new Vector(components));
        Assert.Null(exception);
    }

    [Fact]
    public void LengthTest()
    {
        const double epsilon = 1.0e-10;

        var array = new double[] { 3, 3, 1 };
        var vectorLength = 4.358898943540674;

        var vector = new Vector(array);
        Assert.True(Math.Abs(vector.GetLength() - vectorLength) <= epsilon, $"Длина вектора не совпадает с {vectorLength}");
    }
}