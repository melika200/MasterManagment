namespace DiscountManagement.Application.Contracts.Discount;

public class DiscountViewModel
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Rate { get; set; }
    public bool IsInPercent { get; set; }
    public decimal? Amount { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public bool IsActive { get; set; }
}
