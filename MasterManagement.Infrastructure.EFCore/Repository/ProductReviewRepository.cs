using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.ProductReviewAgg;
using MasterManagement.Infrastructure.EFCore.Context;

namespace MasterManagement.Infrastructure.EFCore.Repository;

public class ProductReviewRepository : RepositoryBase<long, ProductReview>, IProductReviewRepository
{
    private readonly MasterContext _context;

    public ProductReviewRepository(MasterContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ProductReview>> GetByProductIdAsync(long productId)
    {
        var reviews = await GetManyAsync(r => r.ProductId == productId && !r.IsDeleted);
        return reviews.ToList();
    }

}
