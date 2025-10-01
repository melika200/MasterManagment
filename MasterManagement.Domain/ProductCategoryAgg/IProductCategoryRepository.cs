using _01_FrameWork.Domain;
using MasterManagment.Application.Contracts.ProductCategory;

namespace MasterManagement.Domain.ProductCategoryAgg;

public interface IProductCategoryRepository : IRepository<long, ProductCategory>
{

    //List<ProductCategoryViewModel> GetProductCategories();
    EditProductCategoryCommand ?GetDetails(long id);
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
    Task<ProductCategory?> GetById(long id);

    //List<ProductCategory> GetAll();

}
