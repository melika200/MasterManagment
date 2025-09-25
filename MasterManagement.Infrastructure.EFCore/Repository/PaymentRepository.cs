using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Infrastructure.EFCore.Context;

namespace MasterManagement.Infrastructure.EFCore.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MasterContext _context;
        private readonly DbSet<Payment> _dbSet;

        public PaymentRepository(MasterContext context)
        {
            _context = context;
            _dbSet = _context.Set<Payment>();
        }

        public async Task<Payment?> GetByTransactionIdAsync(string transactionId)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<List<Payment>> GetPaymentsByOrderIdAsync(long orderId)
        {
            return await _dbSet.Where(p => p.OrderId == orderId).ToListAsync();
        }

        public async Task<Payment?> GetAsync(long id) => await _dbSet.FindAsync(id);

        public async Task CreateAsync(Payment entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task AddAsync(Payment entity) => CreateAsync(entity);

        public async Task UpdateAsync(Payment entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Payment entity)
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

        public async Task DeleteAsync(Expression<Func<Payment, bool>> expression)
        {
            var entities = await _dbSet.Where(expression).ToListAsync();
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Payment>> GetAllAsync() => await _dbSet.ToListAsync();

        public Task<IEnumerable<Payment>> GetManyAsync(Expression<Func<Payment, bool>> expression) =>
            Task.FromResult(_dbSet.Where(expression).AsEnumerable());

        public Task<Payment?> GetAsync(Expression<Func<Payment, bool>> expression) =>
            _dbSet.FirstOrDefaultAsync(expression);

        public async Task<bool> ExistsAsync(Expression<Func<Payment, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }
    }
}
