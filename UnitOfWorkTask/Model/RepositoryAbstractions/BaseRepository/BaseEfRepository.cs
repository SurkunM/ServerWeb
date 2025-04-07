using Microsoft.EntityFrameworkCore;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;

namespace UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;

public abstract class BaseEfRepository<T> : IRepository<T> where T : class
{
    protected DbContext _db;

    protected DbSet<T> _dbSet;

    public BaseEfRepository(DbContext db)
    {
        ArgumentNullException.ThrowIfNull(db);

        _db = db;
        _dbSet = db.Set<T>();
    }

    public virtual void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void Delete(T entity)
    {
        if (_db.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Attach(entity);
        _db.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Save()
    {
        _db.SaveChanges();
    }

    public virtual T[] GetAll()
    {
        return _dbSet.ToArray();
    }

    public virtual T? GetById(int id)
    {
        return _dbSet.Find(id);
    }
}