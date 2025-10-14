using System.ComponentModel.DataAnnotations;

namespace MasterManagment.Application.Contracts.ProductReview;

public class CreateProductReviewCommand
{
    [Required]
    public long ProductId { get; set; }

    [Required]
    public long AccountId { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MaxLength(1000)]
    public string Message { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }
}
