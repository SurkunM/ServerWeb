using System.Text;

namespace VectorTask;

public class Vector
{
    private double[] _components;

    public int Size => _components.Length;

    public double this[int index]
    {
        get
        {
            if (index < 0 || index >= _components.Length)
            {
                throw new IndexOutOfRangeException($"Индекс \"{index}\" находится за пределами границ вектора от 0 до {_components.Length - 1}");
            }

            return _components[index];
        }

        set
        {
            if (index < 0 || index >= _components.Length)
            {
                throw new IndexOutOfRangeException($"Индекс \"{index}\" находится за пределами границ вектора от 0 до {_components.Length - 1}");
            }

            _components[index] = value;
        }
    }

    public Vector(int dimension)
    {
        if (dimension <= 0)
        {
            throw new ArgumentException($"Размерность вектора не может быть равна \"{dimension}\", данное значение должно быть больше нуля.", nameof(dimension));
        }

        _components = new double[dimension];
    }

    public Vector(Vector vector)
    {
        if (vector is null)
        {
            throw new ArgumentNullException(nameof(vector));
        }

        _components = new double[vector._components.Length];

        vector._components.CopyTo(_components, 0);
    }

    public Vector(double[] components)
    {
        if (components is null)
        {
            throw new ArgumentNullException(nameof(components));
        }

        if (components.Length == 0)
        {
            throw new ArgumentException($"Количество входящих компонентов не может быть равно \"{components.Length}\", данное значение должно быть больше нуля.", nameof(components));
        }

        _components = new double[components.Length];

        components.CopyTo(_components, 0);
    }

    public Vector(int dimension, double[] components)
    {
        if (components is null)
        {
            throw new ArgumentNullException(nameof(components));
        }

        if (dimension <= 0)
        {
            throw new ArgumentException($"Размерность вектора не может быть равна \"{dimension}\", данное значение должно быть больше нуля.", nameof(dimension));
        }

        _components = new double[dimension];

        Array.Copy(components, 0, _components, 0, Math.Min(dimension, components.Length));
    }

    public override string? ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append('{');
        const string separator = ", ";

        foreach (double component in _components)
        {
            stringBuilder.Append(component).Append(separator);
        }

        stringBuilder.Remove(stringBuilder.Length - separator.Length, separator.Length);
        stringBuilder.Append('}');

        return stringBuilder.ToString();
    }

    public override int GetHashCode()
    {
        const int prime = 37;
        int hash = 1;

        foreach (double component in _components)
        {
            hash = prime * hash + component.GetHashCode();
        }

        return hash;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(obj, this))
        {
            return true;
        }

        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        Vector vector = (Vector)obj;

        if (_components.Length != vector._components.Length)
        {
            return false;
        }

        for (int i = 0; i < _components.Length; i++)
        {
            if (_components[i] != vector._components[i])
            {
                return false;
            }
        }

        return true;
    }

    public void Add(Vector vector)
    {
        if (vector is null)
        {
            throw new ArgumentNullException(nameof(vector));
        }

        if (_components.Length < vector._components.Length)
        {
            Array.Resize(ref _components, vector._components.Length);
        }

        for (int i = 0; i < vector._components.Length; i++)
        {
            _components[i] += vector._components[i];
        }
    }

    public void Subtract(Vector vector)
    {
        if (vector is null)
        {
            throw new ArgumentNullException(nameof(vector));
        }

        if (_components.Length < vector._components.Length)
        {
            Array.Resize(ref _components, vector._components.Length);
        }

        for (int i = 0; i < vector._components.Length; i++)
        {
            _components[i] -= vector._components[i];
        }
    }

    public void MultiplyByScalar(double scalar)
    {
        for (int i = 0; i < _components.Length; i++)
        {
            _components[i] *= scalar;
        }
    }

    public void Reverse()
    {
        MultiplyByScalar(-1);
    }

    public double GetLength()
    {
        double sum = 0;

        foreach (double component in _components)
        {
            sum += component * component;
        }

        return Math.Sqrt(sum);
    }

    public static Vector GetSum(Vector vector1, Vector vector2)
    {
        if (vector1 is null)
        {
            throw new ArgumentNullException(nameof(vector1));
        }

        if (vector2 is null)
        {
            throw new ArgumentNullException(nameof(vector2));
        }

        Vector resultVector = new Vector(vector1);

        resultVector.Add(vector2);

        return resultVector;
    }

    public static Vector GetDifference(Vector vector1, Vector vector2)
    {
        if (vector1 is null)
        {
            throw new ArgumentNullException(nameof(vector1));
        }

        if (vector2 is null)
        {
            throw new ArgumentNullException(nameof(vector2));
        }

        Vector resultVector = new Vector(vector1);

        resultVector.Subtract(vector2);

        return resultVector;
    }

    public static double GetScalarProduct(Vector vector1, Vector vector2)
    {
        if (vector1 is null)
        {
            throw new ArgumentNullException(nameof(vector1));
        }

        if (vector2 is null)
        {
            throw new ArgumentNullException(nameof(vector2));
        }

        double scalarProduct = 0;

        int minSize = Math.Min(vector1._components.Length, vector2._components.Length);

        for (int i = 0; i < minSize; i++)
        {
            scalarProduct += vector1._components[i] * vector2._components[i];
        }

        return scalarProduct;
    }
}