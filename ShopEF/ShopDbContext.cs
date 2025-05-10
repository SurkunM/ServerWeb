using Microsoft.EntityFrameworkCore;
using ShopEF.Model;

namespace ShopEF;

public class ShopDbContext : DbContext
{
    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

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
            .HasMaxLength(100);

        modelBuilder.Entity<Product>(b =>
        {
            b.Property(p => p.Name)
                .HasMaxLength(100);

            b.Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CategoryProduct>(b =>
        {
            b.HasOne(cp => cp.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(cp => cp.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(cp => cp.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Customer>(b =>
        {
            b.Property(b => b.FirstName)
                .HasMaxLength(100);

            b.Property(b => b.LastName)
                .HasMaxLength(100);

            b.Property(b => b.MiddleName)
                .HasMaxLength(100);

            b.Property(b => b.Phone)
                .HasMaxLength(100);

            b.Property(b => b.Email)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.HasOne(c => c.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderProduct>(b =>
        {
            b.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(op => op.Product)
                .WithMany(p => p.ProductOrders)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}