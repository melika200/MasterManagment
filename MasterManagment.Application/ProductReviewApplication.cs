using _01_FrameWork.Application;
using MasterManagement.Domain.ProductReviewAgg;
using MasterManagment.Application.Contracts.ProductReview;
using MasterManagment.Application.Contracts.UnitOfWork;

namespace MasterManagement.Application;

public class ProductReviewApplication : IProductReviewApplication
{
    private readonly IProductReviewRepository _repository;
    private readonly IMasterUnitOfWork _unitOfWork;

    public ProductReviewApplication(IProductReviewRepository repository, IMasterUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> CreateProductReviewAsync(CreateProductReviewCommand command,long accountId)
    {
        var operation = new OperationResult();

        var review = new ProductReview(
            command.ProductId,
            accountId,
            command.FullName,
            command.Email,
            command.Message,
            command.Rating
        );

        await _repository.CreateAsync(review);
        await _unitOfWork.CommitAsync();

        return operation.Succedded("نظر شما با موفقیت ثبت شد و پس از تأیید نمایش داده می‌شود.");
    }

    public async Task<List<ProductReviewViewModel>> GetProductReviewByProductIdAsync(long productId)
    {
        var list = await _repository.GetManyAsync(r => r.ProductId == productId && r.IsConfirmed && !r.IsDeleted);
        return list.Select(r => new ProductReviewViewModel
        {
            Id = r.Id,
            ProductId = r.ProductId,
            FullName = r.FullName,
            Email = r.Email,
            Message = r.Message,
            Rating = r.Rating,
            IsConfirmed = r.IsConfirmed,
            CreationDate = r.CreationDate.ToString("yyyy/MM/dd")
        }).ToList();
    }

    public async Task<List<ProductReviewViewModel>> GetAllProductReviewAsync()
    {
        var list = await _repository.GetManyAsync(r => !r.IsDeleted);
        return list.Select(r => new ProductReviewViewModel
        {
            Id = r.Id,
            ProductId = r.ProductId,
            FullName = r.FullName,
            Email = r.Email,
            Message = r.Message,
            Rating = r.Rating,
            IsConfirmed = r.IsConfirmed,
            CreationDate = r.CreationDate.ToString("yyyy/MM/dd")
        }).ToList();
    }

    public async Task<OperationResult> ConfirmProductReviewAsync(long id)
    {
        var operation = new OperationResult();
        var review = await _repository.GetAsync(id);
        if (review == null)
            return operation.Failed("نظر یافت نشد.");

        review.Confirm();
        await _unitOfWork.CommitAsync();
        return operation.Succedded("نظر تأیید شد.");
    }

    public async Task<OperationResult> UnConfirmProductReviewAsync(long id)
    {
        var operation = new OperationResult();
        var review = await _repository.GetAsync(id);
        if (review == null)
            return operation.Failed("نظر یافت نشد.");

        review.UnConfirm();
        await _unitOfWork.CommitAsync();
        return operation.Succedded("نظر لغو تأیید شد.");
    }

    public async Task<OperationResult> RemoveProductReviewAsync(long id)
    {
        var operation = new OperationResult();
        var review = await _repository.GetAsync(id);
        if (review == null)
            return operation.Failed("نظر یافت نشد.");

        review.SoftDelete();
        await _unitOfWork.CommitAsync();
        return operation.Succedded("نظر حذف شد.");
    }
}
