namespace DiscountManagement.ApplicationContracts.DiscountUnitOfWork;


public interface IDiscountUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    //TRepository GetRepository<TRepository>() where TRepository : class;
}

