using Microsoft.EntityFrameworkCore;
using ShopEF.Model;

namespace ShopEF;

class ShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Buyer> Buyers { get; set; }

    public DbSet<OrderProduct> OrderProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=.;Database=Shop;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)//Настроить поля остальных таблиц так же как у Category
    {
        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Product>()
            .HasOne(c => c.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(c => c.CategoryId);

        modelBuilder.Entity<Order>()
            .HasOne(c => c.Buyer)
            .WithMany(c => c.Orders)
            .HasForeignKey(c => c.BuyerId);

        modelBuilder.Entity<OrderProduct>(b =>
        {
            b.HasOne(po => po.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(po => po.OrderId);

            b.HasOne(po => po.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(po => po.ProductId);
        });
    }
}