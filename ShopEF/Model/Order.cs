namespace ShopEF.Model;

class Order
{
    public int Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public List<int>? ProductsId { get; set; }

    public List<Product>? Products { get; set; }

    public int BuyerId { get; set; }

    public Buyer? Buyer { get; set; }
}