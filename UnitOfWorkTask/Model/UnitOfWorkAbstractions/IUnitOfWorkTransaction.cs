namespace UnitOfWorkTask.Model.UnitOfWorkAbstractions;

public interface IUnitOfWorkTransaction
{
    void BeginTransaction();

    void RollbackTransaction();
}
