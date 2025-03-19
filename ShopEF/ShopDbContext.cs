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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            //.LogTo(Console.WriteLine)
            .UseSqlServer("Server=.;Database=Shop;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Product>(b =>
        {
            b.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            b.Property(p => p.Price)
                .HasColumnType("decimal(18, 2)")
                .IsRequired(false);

            b.HasOne(c => c.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Buyer>(b =>
        {
            b.Property(b => b.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            b.Property(b => b.LastName)
                .IsRequired()
                .HasMaxLength(100);

            b.Property(b => b.MiddleName)
                .IsRequired()
                .HasMaxLength(100);

            b.Property(b => b.Phone)
                .IsRequired()
                .HasMaxLength(100);

            b.Property(b => b.Email)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.Property(o => o.OrderDate)
                .HasColumnType("DATETIME2")
                .IsRequired(false);

            b.HasOne(c => c.Buyer)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderProduct>(b =>
        {
            b.HasOne(po => po.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(po => po.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(po => po.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}