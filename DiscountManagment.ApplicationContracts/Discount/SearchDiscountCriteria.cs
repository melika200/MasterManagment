namespace DiscountManagement.Application.Contracts.Discount;

public class SearchDiscountCriteria
{
    public string? Title { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
