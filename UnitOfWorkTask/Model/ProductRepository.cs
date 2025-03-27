using UnitOfWorkTask.Model.RepositoryAbstractions;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using Microsoft.EntityFrameworkCore;
using ShopEF.Model;

namespace UnitOfWorkTask.Model;

public class ProductRepository : BaseEfRepository<Product>, IProductRepository
{
    public ProductRepository(DbContext db) : base(db) { }

    public Product? GetMostPurchasedProduct()
    {
        return _dbSet
            .Include(p => p.OrderProducts)
            .OrderByDescending(p => p.OrderProducts.Count)
            .FirstOrDefault();
    }
}