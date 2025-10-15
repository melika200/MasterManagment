using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.AboutUs.Agg;
using MasterManagement.Domain.AboutUsAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repositories
{
    public class AboutRepository : RepositoryBase<long, About>, IAboutRepository
    {
        private readonly MasterContext _context;
        private readonly DbSet<About> _dbSet;

        public AboutRepository(MasterContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<About>();
        }

        public async Task<About?> GetActiveAbout()
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IsActive);
        }
    }
}
