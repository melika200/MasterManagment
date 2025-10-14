namespace MasterManagment.Application.Contracts.ProductReview;

public class ProductReviewViewModel
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Message { get; set; }
    public int Rating { get; set; }
    public bool IsConfirmed { get; set; }
    public string? CreationDate { get; set; }
}
