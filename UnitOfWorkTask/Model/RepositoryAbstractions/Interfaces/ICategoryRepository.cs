using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Dictionary<Category, int>? GetCategoryAndPurchasedProductsCountDictionary();
}