using _01_FrameWork.Application;

namespace MasterManagment.Application.Contracts.Product;


public interface IProductApplication
{
    Task<OperationResult> CreateAsync(CreateProductCommand command);
    Task<ProductEditResponseCommand> Edit(EditProductCommand command);
    Task<ProductViewModel> GetDetails(long id);
    Task<List<ProductViewModel>> Search(ProductSearchCriteria searchModel);
    Task<OperationResult> DeleteAsync(long id);
    Task<List<ProductViewModel>> GetAllProductsWithCategory();
}
