namespace UnitOfWorkTask.Model.Entities;

public class ProductCategory
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public virtual required Category Category { get; set; }

    public int ProductId { get; set; }

    public virtual required Product Product { get; set; }
}