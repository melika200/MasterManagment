using _01_FrameWork.Domain;

namespace MasterManagement.Domain.ProductReviewAgg;

public interface IProductReviewRepository : IRepository<long, ProductReview>
{
    Task<List<ProductReview>> GetByProductIdAsync(long productId);
}
