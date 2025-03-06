using System.Diagnostics.CodeAnalysis;

namespace CountriesJson.Country
{
    internal class CurrencyComparer : IEqualityComparer<Currency[]>
    {
        public bool Equals(Currency[]? x, Currency[]? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null || x.GetType() != y.GetType())
            {
                return false;
            }

            if (x.Length != y.Length)
            {
                return false;
            }

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i].Name != y[i].Name || x[i].Code != y[i].Code || x[i].Symbol != y[i].Symbol)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode([DisallowNull] Currency[] obj)
        {
            const int prime = 37;
            int hash = 1;

            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] is null)
                {
                    hash = prime * hash;
                }
                else
                {
                    if (obj[i].Name is null)
                    {
                        hash = prime * hash;
                    }
                    else
                    {
                        hash = prime * hash + obj[i].Name.GetHashCode();
                    }

                    if (obj[i].Code is null)
                    {
                        hash = prime * hash;
                    }
                    else
                    {
                        hash = prime * hash + obj[i].Code.GetHashCode();
                    }

                    if (obj[i].Symbol is null)
                    {
                        hash = prime * hash;
                    }
                    else
                    {
                        hash = prime * hash + obj[i].Symbol.GetHashCode();
                    }
                }
            }

            return hash;
        }
    }
}