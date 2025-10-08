namespace MasterManagment.Application.Contracts.Shipping;

public interface IShippingApplication
{
    Task<long> CreateAsync(CreateShippingCommand command);
    Task<ShippingViewModel?> GetByCartIdAsync(long cartId);
}
