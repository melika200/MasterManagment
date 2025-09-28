using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using _01_FrameWork.Domain;

namespace _01_FrameWork.Infrastructure
{
    public class RepositoryBase<TKey, T> : IRepository<TKey, T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }

    

        public async Task CreateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity);
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity);
        }

     

        public async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
            await Task.CompletedTask; 
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

     

        public async Task DeleteAsync(Expression<Func<T, bool>> expression)
        {
            var entities = _dbSet.Where(expression);
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
        public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetAsync((TKey)id);
            if (entity != null)
                await DeleteAsync(entity);
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

    }
}
