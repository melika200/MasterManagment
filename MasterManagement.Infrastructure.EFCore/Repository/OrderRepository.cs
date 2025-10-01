using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using MasterManagment.Application.Contracts.Order;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository;

public class OrderRepository : RepositoryBase<long, Order>, IOrderRepository
{
    private readonly MasterContext _context;
    private readonly DbSet<Order> _dbSet;

    public OrderRepository(MasterContext context):base(context)
    {
        _context = context;
        _dbSet = _context.Set<Order>();
    }

    public async Task<Order?> GetOrderWithItemAsync(long orderId)
    {
        return await _dbSet.Include(o => o.Items)
                           .FirstOrDefaultAsync(o => o.Id == orderId);
    }
   

    public async Task<List<Order>> SearchAsync(OrderSearchCriteria criteria)
    {
        IQueryable<Order> query = _dbSet.Include(o => o.Items);

        if (criteria.AccountId != 0)
            query = query.Where(o => o.AccountId == criteria.AccountId);

        if (criteria.IsCanceled.HasValue)
            query = query.Where(o => o.IsCanceled == criteria.IsCanceled.Value);

        if (criteria.IsPaid.HasValue)
            query = query.Where(o => o.IsPaid == criteria.IsPaid.Value);

        return await query.ToListAsync();
    }

   


  

    //public Task AddAsync(Order entity) => CreateAsync(entity);

  
}
