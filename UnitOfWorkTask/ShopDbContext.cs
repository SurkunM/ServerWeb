using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask;

class ShopDbContext : DbContext
{
    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer("Server=.;Database=Shop;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
             .Property(c => c.Name)
             .HasMaxLength(50);

        modelBuilder.Entity<Customer>(b =>
        {
            b.Property(b => b.FirstName)
                .HasMaxLength(50);

            b.Property(b => b.LastName)
                .HasMaxLength(50);

            b.Property(b => b.MiddleName)
                .HasMaxLength(50);

            b.Property(b => b.Phone)
                .HasMaxLength(50);

            b.Property(b => b.Email)
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>(b =>
        {
            b.Property(b => b.Name)
                .HasMaxLength(50);

            b.Property(b => b.Price)
                .HasColumnType("decimal(18, 2)");

            b.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderProduct>(b =>
        {
            b.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
