using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;
using MasterManagment.Application.Contracts.ProductCategory;

namespace MasterManagement.Domain.ProductCategoryAgg
{
    public interface IProductCategoryRepository : IRepository<long, ProductCategory>
    {

        //List<ProductCategoryViewModel> GetProductCategories();
        EditProductCategoryCommand GetDetails(long id);
        List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
        Task<ProductCategory> GetById(long id);

        //List<ProductCategory> GetAll();

    }
}
