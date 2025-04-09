using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask.Model.Repositories;

public class CategoryRepository : BaseEfRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ShopDbContext db) : base(db) { }

    public Dictionary<Category, int> GetCategoryAndPurchasedProductsCountDictionary()
    {
        return _db.Set<OrderProduct>()
            .Include(op => op.Product)
                .ThenInclude(p => p.Category)
            .GroupBy(op => op.Product.Category)
            .ToDictionary(c => c.Key, op => op.Sum(op => op.ProductCount));
    }
}