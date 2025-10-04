using MasterManagment.Application.Contracts.OrderItem;

namespace MasterManagment.Application.Contracts.Order;

public class OrderDetailViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountPhone { get; set; } = string.Empty;
    public string AccountAddress { get; set; } = string.Empty;
    public int PaymentMethod { get; set; }
    public double TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCanceled { get; set; }
    public string IssueTrackingNo { get; set; } = string.Empty;
    public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
}
