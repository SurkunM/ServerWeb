namespace UnitOfWorkTask.Model.Entities;

public class Customer
{
    public int Id { get; set; }

    public required string LastName { get; set; }

    public required string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public required string Phone { get; set; }

    public required string Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}