using ShopEF.Model;

namespace UnitOfWorkTask.Model.RepositoryAbstractions;

public interface IProductRepository : IRepository<Product>
{
    Product? GetMostPurchasedProduct();
}