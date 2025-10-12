namespace MasterManagment.Application.Contracts.OrderContracts;

public class CartViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string? AccountName { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public double TotalAmount { get; set; }
    public double DiscountAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCanceled { get; set; }
    public string? IssueTrackingNo { get; set; }
}
