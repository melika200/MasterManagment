using MasterManagment.Application.Contracts.OrderItem;

namespace MasterManagment.Application.Contracts.OrderContracts;

public class OrderDetailViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int PaymentMethod { get; set; }
    public double TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCanceled { get; set; }
    public string IssueTrackingNo { get; set; } = string.Empty;
    public string? OrderState { get; set; }
    public string? ShippingState { get; set; }
    //public string OrderStateName { get; set; } = string.Empty;
    //public string ShippingStatusName { get; set; } = string.Empty;
    public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
}

