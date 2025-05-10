namespace ShopEF.Model;

public class Category
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();
}