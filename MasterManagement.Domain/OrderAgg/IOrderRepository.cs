using _01_FrameWork.Domain;
using MasterManagment.Application.Contracts.Order;

namespace MasterManagement.Domain.OrderAgg;

public interface IOrderRepository : IRepository<long, Order>
{
    Task<Order?> GetOrderWithItemAsync(long orderId);
    Task<List<Order>> SearchAsync(OrderSearchCriteria criteria);
}
