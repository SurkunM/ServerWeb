using UnitOfWorkTask.Model.RepositoryAbstractions;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using Microsoft.EntityFrameworkCore;
using ShopEF.Model;

namespace UnitOfWorkTask.Model;

public class OrderRepository : BaseEfRepository<Order>, IOrderRepository
{
    public OrderRepository(DbContext db) : base(db) { }

    public Dictionary<int, decimal> GetBuyersAndSpentMoneySumDictionary()
    {
        return _dbSet
            .Include(o => o.OrderProducts)
            .Select(o => new
            {
                Id = o.BuyerId,
                Sum = o.OrderProducts.Sum(op => op.Product!.Price)
            })
            .ToDictionary(b => b.Id, b => b.Sum);
    }
}