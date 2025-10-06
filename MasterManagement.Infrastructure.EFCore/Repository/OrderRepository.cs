using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using MasterManagment.Application.Contracts.Order;
using MasterManagment.Application.Contracts.ProductCategory;
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



    public async Task<List<OrderViewModel>> SearchAsync(OrderSearchCriteria criteria)
    {
        IQueryable<Order> query = _dbSet.Include(o => o.Items);

        if (criteria.OrderId != 0)
            query = query.Where(o => o.Id == criteria.OrderId);

        if (criteria.AccountId != 0)
            query = query.Where(o => o.AccountId == criteria.AccountId);

        if (criteria.IsCanceled.HasValue)
            query = query.Where(o => o.IsCanceled == criteria.IsCanceled.Value);

        if (criteria.IsPaid.HasValue)
            query = query.Where(o => o.IsPaid == criteria.IsPaid.Value);

        return await query.Select(o => new OrderViewModel
        {
            Id = o.Id,
            AccountId = o.AccountId,
            AccountPhone = null,  
            AccountAddress = null,
            AccountName = null,
            PaymentMethod = (int)o.PaymentMethod,
            TotalAmount = o.TotalAmount,
            IsPaid = o.IsPaid,
            IsCanceled = o.IsCanceled,
            IssueTrackingNo = o.IssueTrackingNo
        }).ToListAsync();
    }



    public async Task<List<OrderViewModel>> GetAllOrdersAsync()
    {
        return await _dbSet.Select(order => new OrderViewModel
        {
            Id = order.Id,
            AccountId = order.AccountId,
            AccountPhone = null,
            AccountAddress = null,  
            PaymentMethod = (int)order.PaymentMethod,
            TotalAmount = order.TotalAmount,
            IsPaid = order.IsPaid,
            IsCanceled = order.IsCanceled,
            IssueTrackingNo = order.IssueTrackingNo
        }).ToListAsync();
    }






    //public Task AddAsync(Order entity) => CreateAsync(entity);


}
