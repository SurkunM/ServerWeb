using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask.Model.Repositories;

public class OrderRepository : BaseEfRepository<Order>, IOrderRepository
{
    public OrderRepository(DbContext db) : base(db) { }

    public Dictionary<int, decimal> GetCustomersAndSpentMoneySumDictionary()
    {
        return _dbSet
            .Include(o => o.OrderProducts)
            .Select(o => new
            {
                Id = o.CustomerId,
                Sum = o.OrderProducts.Sum(op => op.Product!.Price)
            })
            .ToDictionary(b => b.Id, b => b.Sum);
    }
}