using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagment.Contracts.UnitOfWork;

namespace AccountManagement.Infrastructure.EFCore;

public class AccountUnitOfWork : IAccountUnitOfWork
{
    private readonly AccountContext _context;
    //private readonly IServiceProvider _serviceProvider;

    public AccountUnitOfWork(AccountContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    //public TRepository GetRepository<TRepository>() where TRepository : class
    //{
    //    return _serviceProvider.GetRequiredService<TRepository>();
    //}
}
