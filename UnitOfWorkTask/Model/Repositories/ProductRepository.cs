using Microsoft.EntityFrameworkCore;
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
            .Include(op => op.Product)
            .OrderByDescending(op => op.ProductCount)
            .Select(op => op.Product)
            .FirstOrDefault();
    }
}