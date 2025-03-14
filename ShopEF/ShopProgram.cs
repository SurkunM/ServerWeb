namespace ShopEF;

internal class ShopProgram
{
    static void Main(string[] args)
    {
        using var db = new ShopContext();

        //db.Database.EnsureCreated();

        //db.Database.EnsureDeleted();
    }
}