namespace UnitOfWorkTask.Model.Entities;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public virtual required Category Category { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}