using VectorTask;

namespace UnitTestVector
{
    [TestClass]
    public sealed class VectorTests
    {
        [TestMethod]
        public void TestLength()
        {
            double[] array2 = { 3, 3, 1 };
            var vector = new Vector(array2);

            Assert.IsTrue(vector.GetLength() == 4.358898943540674, "Length must be 3");
        }
    }
}
