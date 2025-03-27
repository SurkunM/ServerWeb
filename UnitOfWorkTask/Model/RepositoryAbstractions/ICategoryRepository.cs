using ShopEF.Model;

namespace UnitOfWorkTask.Model.RepositoryAbstractions;

public interface ICategoryRepository : IRepository<Category>
{
    Dictionary<Category, int>? GetCategoryAndPurchasedProductsCountDictionary();
}