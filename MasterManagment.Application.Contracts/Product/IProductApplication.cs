using System.Security.Claims;
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
    Task<List<ProductViewModel>> GetPopularProducts(int count = 10);
    Task<List<ProductViewModel>> GetNewestProducts(int count = 10);
    Task<OperationResult> RateProduct(long productId, int rating, string? comment, ClaimsPrincipal user);

}
