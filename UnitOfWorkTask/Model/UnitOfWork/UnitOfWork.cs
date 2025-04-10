using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWorkTask.Model.UnitOfWorkAbstractions;

namespace UnitOfWorkTask.Model.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private bool _disposed;

    private readonly ShopDbContext _db;

    private readonly IServiceProvider _serviceProvider;

    private IDbContextTransaction? _transaction;

    public UnitOfWork(ShopDbContext db, IServiceProvider serviceProvider)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));

        _serviceProvider = serviceProvider;
        _disposed = false;
    }

    public T GetRepository<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public void BeginTransaction()
    {
        ThrowExceptionIfDisposed();

        if (_transaction != null)
        {
            throw new InvalidOperationException("Транзакция уже создана");
        }

        _transaction = _db.Database.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        ThrowExceptionIfDisposed();

        if (_transaction is not null)
        {
            _transaction.Rollback();
            _transaction = null;
        }
    }

    public void Save()
    {
        ThrowExceptionIfDisposed();

        if (_transaction is not null)
        {
            _transaction.Commit();
            _transaction = null;
        }

        _db.SaveChanges();
    }

    private void ThrowExceptionIfDisposed()
    {
        if (!_disposed)
        {
            return;
        }

        throw new ObjectDisposedException(nameof(_disposed));
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        RollbackTransaction();

        _db.Dispose();
        _disposed = true;
    }
}