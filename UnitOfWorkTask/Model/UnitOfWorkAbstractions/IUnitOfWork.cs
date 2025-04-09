namespace UnitOfWorkTask.Model.UnitOfWorkAbstractions;

public interface IUnitOfWork : IDisposable
{
    void Save();

    T GetRepository<T>() where T : class;
}