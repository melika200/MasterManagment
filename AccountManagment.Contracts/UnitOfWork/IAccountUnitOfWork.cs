namespace AccountManagment.Contracts.UnitOfWork;

public interface IAccountUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    //TRepository GetRepository<TRepository>() where TRepository : class;
}
