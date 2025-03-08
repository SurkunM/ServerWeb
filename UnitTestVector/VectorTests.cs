using VectorTask;

namespace UnitTestVector
{
    [TestClass]
    public sealed class VectorTests
    {
        [TestMethod]
        public void LengthTest()
        {
            const double epsilon = 1.0e-10;
            double[] array = { 3, 3, 1 };
            double vectorLength = Math.Sqrt(array.Sum(e => Math.Pow(e, 2)));

            var vector = new Vector(array);
            Assert.IsTrue(vector.GetLength() - vectorLength <= epsilon, $"Длинна вектора не совпадает с {vectorLength}");
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Action action = () => new Vector(0);
            Assert.ThrowsExactly<ArgumentException>(action);
        }

        [TestMethod]
        public void MultiplyByScalarTest()
        {
            double[] array = { 3, 3, 1 };

            var vector = new Vector(array);
            var scalar = 2;

            for (int i = 0; i < array.Length; i++)
            {
                array[i] *= scalar;
            }

            vector.MultiplyByScalar(scalar);

            Assert.AreEqual(vector, new Vector(array));
        }
    }
}
