using _01_FrameWork.Application;
using MasterManagment.Application.Contracts.OrderItem;
namespace MasterManagment.Application.Contracts.OrderContracts
{
    public interface IOrderApplication
    {
        Task<OperationResult> CreateAsync(CreateOrderCommand command);
        Task<OperationResult> EditAsync(EditOrderCommand command);
        Task<OperationResult> CancelAsync(long id);
        Task<double> GetAmountByAsync(long id);
        //Task<List<OrderItemViewModel>> GetOrderItemsAsync(long orderId);
        Task<OrderDetailViewModel?> GetOrderDetailAsync(long orderId);
        Task<List<OrderViewModel>> SearchAsync(OrderSearchCriteria searchModel);
        Task<long> FinalizeFromCartAsync(long cartId, string transactionId);
        Task<List<OrderViewModel>> GetOrders();
        //Task<OrderViewModel?> GetOrderAsync(long orderId);
        Task<OperationResult> SetTrackingNumberAsync(long orderId, string trackingNumber);
        Task<OperationResult> SetOrderStateAsync(long orderId, int newStateId);
        Task<OperationResult> SetOrderShippingStateAsync(long orderId, int newShippingStateId);
        Task<List<OrderViewModel>> GetOrdersByAccountAsync(long accountId);

    }
}
