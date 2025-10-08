namespace MasterManagment.Application.Contracts.Shipping;

public class EditShippingCommand
{
    public long Id { get; set; } 
    public long CartId { get; set; } 
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ShippingStatusId { get; set; } 
}
