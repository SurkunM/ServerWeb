using UnitOfWorkTask.Model.Entities;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;

namespace UnitOfWorkTask.Model.Repositories;

public class CategoryRepository : BaseEfRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ShopDbContext db) : base(db) { }

    public Dictionary<Category, int> GetCategoryAndPurchasedProductsCountDictionary()
    {
        return _db.Set<ProductCategory>()
            .Select(pc => new
            {
                pc.Category,
                ProductsCount = pc.Product.OrderProducts.Sum(op => op.ProductsCount)
            })
            .GroupBy(a => a.Category)
            .Select(g => new
            {
                Category = g.Key,
                Count = g.Sum(pc => pc.ProductsCount)
            })
            .ToDictionary(g => g.Category, g => g.Count);
    }
}