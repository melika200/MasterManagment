using MasterManagement.Domain.ShippingStatusAgg;

namespace MasterManagement.Domain.ShippingStatusesTypeAgg;

public static class ShippingStatusesType
{
    public static readonly ShippingStatus Preparing = new ShippingStatus(1, "Preparing");
    public static readonly ShippingStatus SentToPost = new ShippingStatus(2, "Sent to Post");
    public static readonly ShippingStatus InTransit = new ShippingStatus(3, "In Transit");
    public static readonly ShippingStatus Delivered = new ShippingStatus(4, "Delivered");
    public static readonly ShippingStatus Returned = new ShippingStatus(5, "Returned");

    public static readonly List<ShippingStatus> AllStatuses = new()
    {
        Preparing,
        SentToPost,
        InTransit,
        Delivered,
        Returned
    };
}
