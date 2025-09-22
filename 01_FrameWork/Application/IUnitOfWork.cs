namespace _01_FrameWork.Application;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    //TRepository GetRepository<TRepository>() where TRepository : class;
}
