namespace CountriesJson.Model;

internal class Currency
{
    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Symbol { get; set; } = default!;

    public override string ToString()
    {
        return $"\"code\": \"{Code}\", \"name\": \"{Name}\", \"symbol\": \"{Symbol}\"";
    }
}