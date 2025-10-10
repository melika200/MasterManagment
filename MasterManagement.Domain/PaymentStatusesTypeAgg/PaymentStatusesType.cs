using MasterManagement.Domain.PaymentStatusAgg;

namespace MasterManagement.Domain.PaymentStatusesTypeAgg;


public static class PaymentStatusesType
{
    public static readonly PaymentStatus Pending = new PaymentStatus(1, "Pending");
    public static readonly PaymentStatus Confirmed = new PaymentStatus(2, "Confirmed");
    public static readonly PaymentStatus Failed = new PaymentStatus(3, "Failed");
    public static readonly PaymentStatus Cancelled = new PaymentStatus(4, "Cancelled");

    public static readonly List<PaymentStatus> AllStatuses = new()
{
    Pending,
    Confirmed,
    Failed,
    Cancelled
};
}
