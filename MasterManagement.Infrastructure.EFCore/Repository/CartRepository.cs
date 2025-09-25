using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using MasterManagement.Domain.OrderAgg;
using MasterManagment.Application.Contracts.Order;

namespace MasterManagement.Infrastructure.EFCore.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly MasterContext _context;
        private readonly DbSet<Cart> _dbSet;

        public CartRepository(MasterContext context)
        {
            _context = context;
            _dbSet = _context.Set<Cart>();
        }

       
        public async Task<Cart?> GetAsync(long id)
        {
            return await _dbSet.Include(c => c.Items)
                               .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart?> GetCartDetailsAsync(long cartId)
        {
            return await _dbSet.Include(c => c.Items)
                               .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<List<Cart>> SearchAsync(CartSearchCriteria criteria)
        {
            IQueryable<Cart> query = _dbSet.Include(c => c.Items);

            if (criteria.AccountId != 0)
                query = query.Where(c => c.AccountId == criteria.AccountId);

            if (criteria.IsCanceled.HasValue)
                query = query.Where(c => c.IsCanceled == criteria.IsCanceled.Value);

            if (criteria.IsPaid.HasValue)
                query = query.Where(c => c.IsPaid == criteria.IsPaid.Value);

            return await query.ToListAsync();
        }

        public async Task CreateAsync(Cart entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
        }

        public Task AddAsync(Cart entity)
        {
            return CreateAsync(entity);
        }

        public async Task UpdateAsync(Cart entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Cart entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetAsync((long)id);
            if (entity != null)
                await DeleteAsync(entity);
        }

        public async Task DeleteAsync(Expression<Func<Cart, bool>> expression)
        {
            var entities = await _dbSet.Where(expression).ToListAsync();
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _dbSet.Include(c => c.Items).ToListAsync();
        }

        public Task<IEnumerable<Cart>> GetManyAsync(Expression<Func<Cart, bool>> expression)
        {
            return Task.FromResult(_dbSet.Where(expression).AsEnumerable());
        }

        public Task<Cart?> GetAsync(Expression<Func<Cart, bool>> expression)
        {
            return _dbSet.Include(c => c.Items).FirstOrDefaultAsync(expression);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Cart, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }
    }
}
