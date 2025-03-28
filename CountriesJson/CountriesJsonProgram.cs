using CountriesJson.Country;
using Newtonsoft.Json;

namespace CountriesJSON;

internal class CountriesJsonProgram
{
    private static void PrintCountriesTotalPopulation(List<Country> countries)
    {
        Console.WriteLine("Суммарная численность населения: {0}", countries.Sum(c => c.Population));
    }

    private static void PrintCountriesCurrencies(List<Country> countries)
    {
        var currenciesList = countries
            .SelectMany(c => c.Currencies)
            .DistinctBy(c => c.Code)
            .ToList();

        Console.WriteLine("Список всех валют из списка стран:");

        foreach (var currency in currenciesList)
        {
            Console.WriteLine(currency);
            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        try
        {
            using var reader = new StreamReader(Path.Combine("..", "..", "..", "CountriesJson", "countries.json"));

            string? countriesJson = reader.ReadToEnd();

            var countries = JsonConvert.DeserializeObject<List<Country>>(countriesJson);

            if (countries is null)
            {
                Console.WriteLine("Список стран пуст");
            }
            else
            {
                PrintCountriesTotalPopulation(countries);
                PrintCountriesCurrencies(countries);
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