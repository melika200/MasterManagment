using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.FaqUs.Agg;
using MasterManagement.Domain.FaqUsAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repositories;

public class FaqRepository : RepositoryBase<long, Faq>, IFaqRepository
{
    private readonly MasterContext _context;
    private readonly DbSet<Faq> _dbSet;

    public FaqRepository(MasterContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Faq>();
    }

    public async Task<List<Faq>> GetActiveFaqs()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(f => f.IsActive)
            .ToListAsync();
    }
}
