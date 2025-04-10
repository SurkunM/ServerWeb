using UnitOfWorkTask.Model.Entities;
using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;

namespace UnitOfWorkTask.Model.Repositories;

public class CustomerRepository : BaseEfRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ShopDbContext db) : base(db) { }
}