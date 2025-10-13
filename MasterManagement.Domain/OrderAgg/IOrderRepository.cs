using _01_FrameWork.Application;
using _01_FrameWork.Domain;
using MasterManagment.Application.Contracts.OrderContracts;

namespace MasterManagement.Domain.OrderAgg;

public interface IOrderRepository : IRepository<long, Order>
{
    Task<Order?> GetOrderWithItemAsync(long orderId);
    Task<List<OrderViewModel>> SearchAsync(OrderSearchCriteria criteria);
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetOrdersByAccountIdAsync(long accountId);



}
