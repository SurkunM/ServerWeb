using UnitOfWorkTask.Model.RepositoryAbstractions;
using UnitOfWorkTask.Model.UnitOfWorkAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace UnitOfWorkTask.Model;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _db;

    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbContext db)
    {
        if (db is null)
        {
            throw new ArgumentNullException(nameof(db));
        }

        _db = db;
    }

    public T? GetRepository<T>() where T : class
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

        if (typeof(T) == typeof(IBuyerRepository))
        {
            return new BuyerRepository(_db) as T;
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
        if (_transaction is not null)
        {
            _transaction.Rollback();
            _transaction = null;
        }

        _db.Dispose();
    }
}