namespace ShopEF.Model;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}