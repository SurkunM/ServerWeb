using UnitOfWorkTask.Model.Entities;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;

namespace UnitOfWorkTask.Model.Repositories;

public class ProductRepository : BaseEfRepository<Product>, IProductRepository
{
    public ProductRepository(ShopDbContext db) : base(db) { }

    public Product? GetMostPurchasedProduct()
    {
        return _db.Set<OrderProduct>()
            .GroupBy(op => op.Product)
            .OrderByDescending(g => g.Sum(op => op.ProductsCount))
            .Select(g => g.Key)
            .FirstOrDefault();
    }
}