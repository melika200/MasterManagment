using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.SliderAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository;

public class SliderRepository : RepositoryBase<long, Slider>, ISliderRepository
{
    private readonly MasterContext _context;
    private readonly DbSet<Slider> _dbSet;

    public SliderRepository(MasterContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Slider>();
    }

    public async Task<List<Slider>> GetActiveSlidersAsync()
    {
        return await _dbSet
            .Where(s => s.IsDeleted == false && s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();
    }

    public async Task<List<Slider>> GetAllForAdminAsync()
    {
        return await _dbSet
            .IgnoreQueryFilters()
            .OrderByDescending(s => s.CreationDate)
            .ToListAsync();
    }

    public async Task<Slider?> GetByIdAsync(long id)
    {
        return await _dbSet
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
