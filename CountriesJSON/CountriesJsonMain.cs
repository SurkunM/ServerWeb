using CountriesJson.Country;
using Newtonsoft.Json;
using System.Text;

namespace CountriesJSON;

internal class CountriesJsonMain
{
    static void Main(string[] args)
    {
        try
        {
            using var reader = new StreamReader("..\\..\\..\\json\\countries.json");

            var stringBuilder = new StringBuilder();

            string? countriesJson;

            while ((countriesJson = reader.ReadLine()) != null)
            {
                stringBuilder.Append(countriesJson);
            }

            countriesJson = stringBuilder.ToString();
            var countries = JsonConvert.DeserializeObject<List<Country>>(countriesJson);

            if (countries is null)
            {
                throw new NullReferenceException(nameof(countries));
            }

            var totalPopulations = countries
                .Select(c => c.Population)
                .Sum();

            Console.WriteLine("Суммарная численность населения: {0}", totalPopulations);

            var currenciesList = countries
                .Select(c => c.Currencies)
                .Distinct(new CurrencyComparer())
                .ToList();

            Console.WriteLine("Список всех валют из списка стран:");

            foreach (var country in currenciesList)
            {
                foreach (var c in country)
                {
                    Console.WriteLine(c);
                    Console.WriteLine();
                }
            }
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Ошибка десериализации данных с файла");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Не найден файл для чтения");
        }
    }
}