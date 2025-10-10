using MasterManagment.Application.Contracts.OrderItem;
using MasterManagment.Application.Contracts.Shipping;

namespace MasterManagment.Application.Contracts.OrderContracts;

public class CreateOrderCommand
{
    public long AccountId { get; set; }
    public long CartId { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public List<OrderItemViewModel>? Items { get; set; }
    public double DiscountAmount { get; set; }
    public double PayAmount { get; set; }
    public double TotalAmount { get; set; }
    public long PaymentId { get; set; }
    public ShippingViewModel? ShippingInfo { get; set; }
}
