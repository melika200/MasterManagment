using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;
using Microsoft.EntityFrameworkCore;

namespace _01_FrameWork.Infrastructure
{
    public class RepositoryBase<TKey, T> : IRepository<TKey, T> where T : class
    {
        private readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Add(entity);
        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);
        }

        public T Get(TKey id)
        {
            return _context.Find<T>(id);
        }

        public List<T> Get()
        {
            return _context.Set<T>().ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        //
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public void Delete(object id)
        {
            var entity = GetBy(id);
            if (entity != null)
                Delete(entity);
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            var entities = GetMany(expression);
            if (entities != null)
                foreach (var item in entities)
                {
                    Delete(item);
                }
        }

        public bool IsExists(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);
        }

        public T? Get(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).FirstOrDefault();
            //return _context.Find<T>(expression); // this is optimized for fetchin by primary key
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public IQueryable<T> GetAllQuery()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public T? GetBy(object id)
        {
            return _context.Find<T>(id);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).ToList();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public void SaveAsync()
        {
            _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public T GetAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAsync()
        {
            throw new NotImplementedException();
        }

        public void CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
