namespace MasterManagment.Application.Contracts.Order;

public class CartViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string? AccountName { get; set; }
    public int PaymentMethod { get; set; }
    public double TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCanceled { get; set; }
    public string? IssueTrackingNo { get; set; }
}
