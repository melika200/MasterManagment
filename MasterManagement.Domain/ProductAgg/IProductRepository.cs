using System.Linq.Expressions;
using _01_FrameWork.Domain;

namespace MasterManagement.Domain.ProductAgg;


public interface IProductRepository : IRepository<long, Product>
{
    Task<IEnumerable<Product>> GetAllProductsWithCategory(Expression<Func<Product, bool>>? where = null);
    Task<IEnumerable<Product>> GetProductsByCategoryId(long categoryId);
    Task<Product?> GetById(long id);


}


