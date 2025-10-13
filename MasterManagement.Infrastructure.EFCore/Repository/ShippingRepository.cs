using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.ShippingAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository;

public class ShippingRepository : RepositoryBase<long, Shipping>, IShippingRepository
{
    private readonly MasterContext _context;
    private readonly DbSet<Order> _dbSet;

    public ShippingRepository(MasterContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Order>();
    }

    public async Task<Shipping?> GetByCartIdAsync(long cartId)
    {
        return await _context.Set<Shipping>().Include(s=>s.ShippingStatus)
            .FirstOrDefaultAsync(x => x.CartId == cartId);
    }
}
