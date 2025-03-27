using ShopEF.Model;

namespace UnitOfWorkTask.Model.RepositoryAbstractions;

public interface IOrderRepository : IRepository<Order>
{
    Dictionary<int, decimal> GetBuyersAndSpentMoneySumDictionary();
}