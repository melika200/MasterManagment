using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.ShippingStatusAgg;
using MasterManagement.Domain.ShippingStatusesTypeAgg;

public class Shipping
{
    public long Id { get; private set; }
    public long CartId { get; private set; }
    public Cart? Cart { get; private set; } = null!;
    public string FullName { get; private set; } = "";
    public string PhoneNumber { get; private set; } = "";
    public string Address { get; private set; } = "";
    public string Description { get; private set; } = "";
    public int ShippingStatusId { get; private set; }
    public ShippingStatus? ShippingStatus { get; private set; }

    public DateTime CreatedAt { get; private set; }


    public Shipping(long cartId, string fullName, string phoneNumber, string address, string description)
    {
        CartId = cartId;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Address = address;
        Description = description;
        ShippingStatusId = ShippingStatusesType.Preparing.Id;
        CreatedAt = DateTime.Now;
    }


    private Shipping() { }
}
