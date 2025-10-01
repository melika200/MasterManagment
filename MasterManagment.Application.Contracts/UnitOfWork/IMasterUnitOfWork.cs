namespace MasterManagment.Application.Contracts.UnitOfWork;

public interface IMasterUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    //TRepository GetRepository<TRepository>() where TRepository : class;
}
