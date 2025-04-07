using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask.Model.Repositories;

public class CategoryRepository : BaseEfRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DbContext db) : base(db) { }

    public Dictionary<Category, int>? GetCategoryAndPurchasedProductsCountDictionary()
    {
        return _dbSet
            .Include(c => c.Products)
                .ThenInclude(p => p.OrderProducts)
            .ToDictionary(c => c, c => c.Products.Sum(p => p.OrderProducts.Count));
    }
}