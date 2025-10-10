namespace MasterManagment.Application.Contracts.OrderContracts;

public class OrderViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string? FullName { get; set; }
    public string? Mobile { get; set; }
    public string? Address { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public double TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCanceled { get; set; }
    public string? IssueTrackingNo { get; set; }
    public string? OrderState { get; set; } 
    public string? ShippingState { get; set; }
    //public string OrderStateName { get; set; } = string.Empty;
    //public string ShippingStatusName { get; set; } = string.Empty;

 

}





