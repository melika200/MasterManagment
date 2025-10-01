using _01_FrameWork.Domain;
using MasterManagement.Domain.OrderAgg;

namespace MasterManagement.Domain.OrderItemAgg;

public interface IOrderItemRepository : IRepository<long, Order>
{
}
