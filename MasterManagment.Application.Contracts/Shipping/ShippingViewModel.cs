namespace MasterManagment.Application.Contracts.Shipping;

 public class ShippingViewModel
{
    public long Id { get; set; }
    public string FullName { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Address { get; set; } = "";
    public string Description { get; set; } = "";
    public int ShippingStatusId { get; set; }
    public string ShippingStatusName { get; set; } = "";
}
