namespace UnitOfWorkTask.Model.UnitOfWorkAbstractions;

public interface IUnitOfWork : IUnitOfWorkTransaction, IDisposable
{
    void Save();

    T GetRepository<T>() where T : class;
}