namespace ShopEF.Model;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<CategoryProduct> ProductCategories { get; set; } = new List<CategoryProduct>();

    public virtual ICollection<OrderProduct> ProductOrders { get; set; } = new List<OrderProduct>();
}