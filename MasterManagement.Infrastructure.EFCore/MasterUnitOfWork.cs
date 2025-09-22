using _01_FrameWork.Application;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.Extensions.DependencyInjection;

namespace MasterManagement.Infrastructure.EFCore;

public class MasterUnitOfWork : IUnitOfWork
{
    private readonly MasterContext _context;
    //private readonly IServiceProvider _serviceProvider;

    public MasterUnitOfWork(MasterContext context)
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
        catch
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
