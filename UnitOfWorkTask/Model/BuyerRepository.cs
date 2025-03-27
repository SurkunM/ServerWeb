using UnitOfWorkTask.Model.RepositoryAbstractions;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using Microsoft.EntityFrameworkCore;
using ShopEF.Model;

namespace UnitOfWorkTask.Model;

public class BuyerRepository : BaseEfRepository<Buyer>, IBuyerRepository
{
    public BuyerRepository(DbContext db) : base(db) { }
}