using Microsoft.EntityFrameworkCore;
using ShopEF.DbTables;

namespace ShopEF;

class ShopContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Buyer> Buyers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=.;Database=Shop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
    }
}