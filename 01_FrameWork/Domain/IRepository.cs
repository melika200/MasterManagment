using System.Linq.Expressions;


namespace _01_FrameWork.Domain;

public interface IRepository<TKey, T> where T : class
{

    Task<T?> GetAsync(TKey id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> expression);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression);
    Task CreateAsync(T entity);
    //Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteAsync(object id);
    Task DeleteAsync(Expression<Func<T, bool>> expression);
    Task<bool> IsExistsAsync(Expression<Func<T, bool>> expression);


}

    //T Get(TKey id);

    //List<T> Get();
    //Task<List<T>> GetAllAsync();
    //void Create(T entity);



    //bool ExistsAsync(Expression<Func<T, bool>> expression);
    //void Add(T entity);

    //IEnumerable<T> GetAll();
    //IEnumerable<T> GetMany(Expression<Func<T, bool>> expression);
    //T? Get(Expression<Func<T, bool>> expression);

    //Task<T?> GetAsync(Expression<Func<T, bool>> expression);
    //void SaveChanges();
    //Task SaveChangesAsync();