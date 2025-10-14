using _01_FrameWork.Application;

namespace MasterManagment.Application.Contracts.ProductReview;

public interface IProductReviewApplication
{
    Task<OperationResult> CreateProductReviewAsync(CreateProductReviewCommand command);
    Task<OperationResult> ConfirmProductReviewAsync(long id);
    Task<OperationResult> UnConfirmProductReviewAsync(long id);
    Task<OperationResult> RemoveProductReviewAsync(long id);
    Task<List<ProductReviewViewModel>> GetProductReviewByProductIdAsync(long productId);
    Task<List<ProductReviewViewModel>> GetAllProductReviewAsync();
}
