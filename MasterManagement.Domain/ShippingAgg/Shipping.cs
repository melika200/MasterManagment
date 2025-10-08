using MasterManagement.Domain.ShippingStatusAgg;

namespace MasterManagement.Domain.ShippingAgg;

public class Shipping
{
    public long Id { get; private set; }
    public long CartId { get; private set; } 
    public string FullName { get; private set; } = "";
    public string PhoneNumber { get; private set; } = "";
    public string Address { get; private set; } = "";
    public string Description { get; private set; } = "";
    public int ShippingStatusId { get; private set; }
    public ShippingStatus? ShippingStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }
}

