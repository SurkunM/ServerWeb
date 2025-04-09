using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask.Model.Repositories;

public class OrderRepository : BaseEfRepository<Order>, IOrderRepository
{
    public OrderRepository(ShopDbContext db) : base(db) { }

    public Dictionary<Customer, decimal> GetCustomersAndSpentMoneySumDictionary()
    {
        return _dbSet
            .Include(o => o.OrderProducts)
            .Include(o => o.Customer)
            .Select(o => new
            {
                Id = o.Customer,
                Sum = o.OrderProducts.Sum(op => op.ProductCount * op.Product.Price)
            })
            .ToDictionary(b => b.Id, b => b.Sum);
    }
}