namespace ShopEF.Model;

public class OrderProduct
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public virtual required Product Product { get; set; }

    public int OrderId { get; set; }

    public int ProductCount { get; set; }

    public virtual required Order Order { get; set; }
}