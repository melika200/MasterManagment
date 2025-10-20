namespace DiscountManagement.Application.Contracts.Discount;

public class CreateDiscountCommand
{
    public string Title { get; set; } = string.Empty;
    public int Rate { get; set; }
    public bool IsInPercent { get; set; } = true;
    public decimal? Amount { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Reason { get; set; }
}
