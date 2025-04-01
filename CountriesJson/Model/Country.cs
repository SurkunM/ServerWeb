namespace CountriesJson.Model;

internal class Country
{
    public string Name { get; set; } = default!;

    public int Population { get; set; }

    public Currency[] Currencies { get; set; } = default!;
}