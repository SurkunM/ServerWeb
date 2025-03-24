namespace ShopEF.Model;

public class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public int BuyerId { get; set; }

    public virtual Buyer? Buyer { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}