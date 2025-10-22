using _01_FrameWork.Domain;
using MasterManagement.Domain.ProductAgg;

namespace MasterManagement.Domain.ProductReviewAgg;

public class ProductReview : EntityBase, ISoftDelete
{
    public long ProductId { get; private set; }
    public Product? Product { get; private set; }
    public long AccountId { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Message { get; private set; }
    public int Rating { get; private set; } 
    public bool IsConfirmed { get; private set; }
    public bool IsDeleted { get; set; }

    protected ProductReview() { }

    public ProductReview(long productId, long accountId, string fullName, string email, string message, int rating)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "امتیاز باید بین 1 تا 5 باشد.");
        ProductId = productId;
        AccountId = accountId;
        FullName = fullName;
        Email = email;
        Message = message;
        Rating = rating;
        IsConfirmed = false;
        IsDeleted = false;
    }

    public void Confirm() => IsConfirmed = true;
    public void UnConfirm() => IsConfirmed = false;

    public void Edit(string message, int rating)
    {
        Message = message;
        Rating = rating;
    }

    public void SoftDelete() => IsDeleted = true;
}
