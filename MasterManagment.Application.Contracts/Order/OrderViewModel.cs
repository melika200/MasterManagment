namespace MasterManagment.Application.Contracts.Order;

public class OrderViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string? AccountPhone { get; set; }
    public string? AccountAddress { get; set; }
    public string? AccountName { get; set; }
    public int PaymentMethod { get; set; }
    public double TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCanceled { get; set; }
    public string? IssueTrackingNo { get; set; }
    
}




