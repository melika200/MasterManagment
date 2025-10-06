using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Application;
using MasterManagment.Application.Contracts.OrderItem;

namespace MasterManagment.Application.Contracts.Order
{
    public interface IOrderApplication
    {
        Task<OperationResult> CreateAsync(CreateOrderCommand command);
        Task<OperationResult> EditAsync(EditOrderCommand command);
        Task CancelAsync(long id);
        Task<double> GetAmountByAsync(long id);
        Task<List<OrderItemViewModel>> GetOrderItemsAsync(long orderId);
        Task<List<OrderViewModel>> SearchAsync(OrderSearchCriteria searchModel);
        Task<long> FinalizeFromCartAsync(long cartId, string transactionId);
        Task<List<OrderViewModel>> GetOrders();
    }
}
