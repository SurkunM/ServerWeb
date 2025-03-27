using UnitOfWorkTask.Model.RepositoryAbstractions.BaseRepository;

namespace UnitOfWorkTask.Model.RepositoryAbstractions;

public interface IRepository<T>: IRepository where T : class
{
    void Create(T entity);

    void Update(T entity);

    void Delete(T entity);

    void Save();

    T[] GetAll();

    T? GetById(int id);
}