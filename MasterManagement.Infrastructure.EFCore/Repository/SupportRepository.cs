using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.SupportAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository;

public class SupportRepository : RepositoryBase<long, Support>, ISupportRepository
{
    private readonly MasterContext _context;
    private readonly DbSet<Support> _dbSet;

    public SupportRepository(MasterContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Support>();
    }
    public async Task<List<Support>> GetAllSupport()
    {
        return await _dbSet
            .Include(x => x.Status)
            .OrderByDescending(x => x.CreationDate)
            .ToListAsync();
    }



    //public async Task<List<Support>> SearchAsync(string? keyword, string? status, long? accountId)
    //{
    //    var query = _dbSet.Include(x => x.Status).AsQueryable();

    //    if (!string.IsNullOrWhiteSpace(keyword))
    //        query = query.Where(x => x.FullName.Contains(keyword) || x.Email.Contains(keyword) || x.Subject.Contains(keyword));

    //    if (!string.IsNullOrWhiteSpace(status))
    //        query = query.Where(x => x.Status != null && x.Status.Name == status);

    //    if (accountId.HasValue)
    //        query = query.Where(x => x.AccountId == accountId.Value);

    //    return await query.OrderByDescending(x => x.CreationDate).ToListAsync();
    //}

    public async Task<List<Support>> GetByAccountIdAsync(long accountId)
    {
        return await _dbSet.Include(x => x.Status)
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CreationDate)
            .ToListAsync();
    }
}
