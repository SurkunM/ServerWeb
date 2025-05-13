namespace UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;

public interface IRepository
{
}

public interface IRepository<T>: IRepository where T : class
{
    void Create(T entity);

    void Update(T entity);

    void Delete(T entity);

    T[] GetAll();

    T? GetById(int id);
}