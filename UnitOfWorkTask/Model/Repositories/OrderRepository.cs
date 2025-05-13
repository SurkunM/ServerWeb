using UnitOfWorkTask.Model.Entities;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;

namespace UnitOfWorkTask.Model.Repositories;

public class OrderRepository : BaseEfRepository<Order>, IOrderRepository
{
    public OrderRepository(ShopDbContext db) : base(db) { }

    public Dictionary<Customer, decimal> GetCustomersAndSpentMoneySumDictionary()
    {
        return _db.Set<OrderProduct>()
            .GroupBy(op => op.Order)
            .ToDictionary(o => o.Key.Customer, valueOp => valueOp.Sum(op => op.Product.Price * op.ProductsCount));
    }
}