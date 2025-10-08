using _01_FrameWork.Domain;

namespace MasterManagement.Domain.ShippingAgg;

public interface IShippingRepository : IRepository<long, Shipping>
{
    Task<Shipping?> GetByCartIdAsync(long cartId);
}
