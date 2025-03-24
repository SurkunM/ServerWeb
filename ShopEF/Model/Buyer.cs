namespace ShopEF.Model;

public class Buyer
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime BirthDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}