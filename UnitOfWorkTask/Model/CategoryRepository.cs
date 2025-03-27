using UnitOfWorkTask.Model.RepositoryAbstractions;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using Microsoft.EntityFrameworkCore;
using ShopEF.Model;

namespace UnitOfWorkTask.Model;

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