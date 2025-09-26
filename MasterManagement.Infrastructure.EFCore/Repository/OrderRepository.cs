using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using MasterManagment.Application.Contracts.Order;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MasterContext _context;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(MasterContext context)
        {
            _context = context;
            _dbSet = _context.Set<Order>();
        }

        public async Task<Order?> GetOrderDetailsAsync(long orderId)
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

        public async Task<Order?> GetAsync(long id)
        {
            return await _dbSet.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        }


        public async Task CreateAsync(Order entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task AddAsync(Order entity) => CreateAsync(entity);

        public async Task UpdateAsync(Order entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetAsync((long)id);
            if (entity != null)
                await DeleteAsync(entity);
        }

        public async Task DeleteAsync(Expression<Func<Order, bool>> expression)
        {
            var entities = await _dbSet.Where(expression).ToListAsync();
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync() => await _dbSet.ToListAsync();

        public Task<IEnumerable<Order>> GetManyAsync(Expression<Func<Order, bool>> expression) =>
            Task.FromResult(_dbSet.Where(expression).AsEnumerable());

        public Task<Order?> GetAsync(Expression<Func<Order, bool>> expression) =>
            _dbSet.FirstOrDefaultAsync(expression);

        public async Task<bool> ExistsAsync(Expression<Func<Order, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }
    }
}
