namespace ShopEF.Model;

class Order
{
    public int Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public int BuyerId { get; set; }

    public Buyer? Buyer { get; set; }

    public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}