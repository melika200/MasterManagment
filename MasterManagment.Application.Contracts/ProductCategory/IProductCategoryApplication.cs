using _01_FrameWork.Application;

namespace MasterManagment.Application.Contracts.ProductCategory;

 public interface IProductCategoryApplication
{
    Task<OperationResult> CreateAsync(CreateProductCategoryCommand command);
    Task<OperationResult> EditAsync(EditProductCategoryCommand command);
    Task<ProductCategoryViewModel> GetDetails(long id);
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
    Task<OperationResult> DeleteAsync(long id);
    Task<List<ProductCategoryViewModel>> GetAll();

}
