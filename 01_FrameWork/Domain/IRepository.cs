using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _01_FrameWork.Domain
{
    public interface IRepository<TKey, T> where T : class
    {
        T Get(TKey id);
        T GetAsync(TKey id);
        List<T> Get();
        List<T> GetAsync();
        void Create(T entity);
        void CreateAsync(T entity);
        bool Exists(Expression<Func<T, bool>> expression);
        void Add(T entity);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(object id);
        void Delete(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> expression);
        T? Get(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> expression);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        void SaveChanges();
        void SaveChangesAsync();
    }
}
