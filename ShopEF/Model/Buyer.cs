namespace ShopEF.Model;

class Buyer
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public int? Phone { get; set; }

    public string? Email { get; set; }

    public List<int>? OrdersId { get; set; }

    public List<Order> Orders { get; set; } = new List<Order>();
}