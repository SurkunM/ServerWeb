using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.Entities;

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