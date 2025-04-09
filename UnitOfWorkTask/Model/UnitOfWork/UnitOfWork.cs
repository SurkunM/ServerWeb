using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWorkTask.Model.UnitOfWorkAbstractions;

namespace UnitOfWorkTask.Model.UnitOfWork;

public class UnitOfWork : IUnitOfWorkTransaction //TODO: 11. UnitOfWork - если вызывался Dispose, то все методы (кроме самого Dispose) должны бросать исключение.
{
    private readonly ShopDbContext _db;

    private readonly IServiceProvider _serviceProvider;

    private IDbContextTransaction? _transaction;

    public UnitOfWork(ShopDbContext db, IServiceProvider serviceProvider)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _serviceProvider = serviceProvider;
    }

    public T GetRepository<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
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