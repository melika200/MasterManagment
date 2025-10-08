using MasterManagement.Domain.OrderStateAgg;

namespace MasterManagement.Domain.OrderStatesTypeAgg;


public static class OrderStatesType
{
    public static readonly OrderState Pending = new OrderState(1, "Pending");
    public static readonly OrderState Paid = new OrderState(2, "Paid");
    public static readonly OrderState Shipped = new OrderState(3, "Shipped");
    public static readonly OrderState Canceled = new OrderState(4, "Canceled");

    public static readonly List<OrderState> AllStates = new()
    {
        Pending,
        Paid,
        Shipped,
        Canceled
    };
}



