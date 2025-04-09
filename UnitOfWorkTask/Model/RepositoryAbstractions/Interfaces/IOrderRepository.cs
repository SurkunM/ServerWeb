using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Dictionary<Customer, decimal> GetCustomersAndSpentMoneySumDictionary();
}