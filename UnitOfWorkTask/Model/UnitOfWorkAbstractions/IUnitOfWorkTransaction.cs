namespace UnitOfWorkTask.Model.UnitOfWorkAbstractions;

interface IUnitOfWorkTransaction : IUnitOfWork
{
    void BeginTransaction();

    void RollbackTransaction();
}
