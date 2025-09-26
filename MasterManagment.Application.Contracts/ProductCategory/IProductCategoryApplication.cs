using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Application;

namespace MasterManagment.Application.Contracts.ProductCategory
{
     public interface IProductCategoryApplication
    {
        Task<OperationResult> CreateAsync(CreateProductCategoryCommand command);
        Task<OperationResult> EditAsync(EditProductCategoryCommand command);
        EditProductCategoryCommand GetDetails(long id);
        List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
        Task<OperationResult> DeleteAsync(long id);
    }
}
