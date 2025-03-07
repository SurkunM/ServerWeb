namespace VectorTask;

internal class VectorMain
{
    static void Main(string[] args)
    {
        double[] array1 = { 3, -3, 2, 10 };
        double[] array2 = { 3, 3, 1 };

        Vector vector1 = new Vector(2, array1);
        Vector vector2 = new Vector(array2);
        Console.WriteLine("Длинна вектора2 = {0}", vector2.GetLength());

        Console.WriteLine("Вектор 1: {0}", vector1);
        Console.WriteLine("Вектор 2: {0}", vector2);

        vector1.Add(vector2);
        Console.WriteLine("Сумма векторов = {0}", vector1);

        vector1.Subtract(vector2);
        Console.WriteLine("Разность векторов = {0}", vector1);

        vector1.MultiplyByScalar(2);
        Console.WriteLine("Умножение вектора на скаляр = {0}", vector1);

        vector1.Reverse();
        Console.WriteLine("Разворот {0} мерного вектора = {1}", vector1.Size, vector1);

        Console.WriteLine("Длинна вектора = {0}", vector1.GetLength());
        Console.WriteLine("Первый компонент вектора {0} = {1}", vector1, vector1[0]);

        Vector vectorsSum = Vector.GetSum(vector1, vector2);
        Console.WriteLine("Сумма векторов = {0}", vectorsSum);

        Vector vectorsDifference = Vector.GetDifference(vector1, vector2);
        Console.WriteLine("Разность векторов = {0}", vectorsDifference);

        Console.WriteLine("Скалярное произведение векторов = {0}", Vector.GetScalarProduct(vector1, vector2));
    }
}