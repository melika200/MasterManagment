using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;

namespace MasterManagement.Domain.ProductAgg
{

    public interface IProductRepository : IRepository<long, Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategory(Expression<Func<Product, bool>> where = null);
        Task<IEnumerable<Product>> GetProductsByCategoryId(long categoryId);
        Task<Product?> GetById(long id);

    }
}


