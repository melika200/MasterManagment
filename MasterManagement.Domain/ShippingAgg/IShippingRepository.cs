using _01_FrameWork.Domain;
using MasterManagement.Domain.OrderAgg;

namespace MasterManagement.Domain.ShippingAgg;

public interface IShippingRepository : IRepository<long, Order>
{
}
