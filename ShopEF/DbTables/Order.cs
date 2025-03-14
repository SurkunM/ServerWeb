using System.Data;

namespace ShopEF.DbTables;

class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public List<Product> Products { get; set; }

    public Buyer Buyer { get; set; }
}