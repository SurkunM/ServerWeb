using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask.Model.Repositories;

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