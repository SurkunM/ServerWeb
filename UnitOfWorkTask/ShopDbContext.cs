using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask;

public class ShopDbContext : DbContext
{
    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(b =>
        {
            b.Property(b => b.Name)
                .HasMaxLength(50);

            b.Property(b => b.Price)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Category>()
             .Property(c => c.Name)
             .HasMaxLength(50);

        modelBuilder.Entity<ProductCategory>(b =>
        {
            b.HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

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

        modelBuilder.Entity<OrderProduct>(b =>
        {
            b.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
