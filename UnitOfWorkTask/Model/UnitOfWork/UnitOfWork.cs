using UnitOfWorkTask.Model.UnitOfWorkAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.Repositories;

namespace UnitOfWorkTask.Model.UnitOfWork;

public class UnitOfWork : IUnitOfWork //TODO: 11. UnitOfWork - если вызывался Dispose, то все методы (кроме самого Dispose) должны бросать исключение.
{
    private readonly DbContext _db;

    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public T GetRepository<T>() where T : class
    {
        if (typeof(T) == typeof(ICategoryRepository))
        {
            return new CategoryRepository(_db) as T;
        }

        if (typeof(T) == typeof(IProductRepository))
        {
            return new ProductRepository(_db) as T;
        }

        if (typeof(T) == typeof(IOrderRepository))
        {
            return new OrderRepository(_db) as T;
        }

        if (typeof(T) == typeof(ICustomerRepository))
        {
            return new CustomerRepository(_db) as T;
        }

        throw new Exception("Неизвестный тип репозитория:" + typeof(T));
    }

    public void BeginTransaction()
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Транзакция уже создана");
        }

        _transaction = _db.Database.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        if (_transaction is not null)
        {
            _transaction.Rollback();
            _transaction = null;
        }
    }

    public void Save()
    {
        if (_transaction is not null)
        {
            _transaction.Commit();
            _transaction = null;
        }

        _db.SaveChanges();
    }

    public void Dispose()
    {
        RollbackTransaction();

        _db.Dispose();
    }
}