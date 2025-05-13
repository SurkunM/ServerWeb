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
            .ToDictionary(c => c.Key, a => a.Sum(pc => pc.ProductsCount));
    }
}