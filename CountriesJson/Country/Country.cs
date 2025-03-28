namespace CountriesJson.Country;

internal class Country
{
    public string Name { get; set; } = default!;

    public int Population { get; set; } = default!;

    public Currency[] Currencies { get; set; } = default!;
}