using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Order
{
    public interface IOrderApplication
    {
        Task<long> CreateAsync(CreateOrderCommand command);
        Task EditAsync(EditOrderCommand command);
        Task CancelAsync(long id);
        Task<double> GetAmountByAsync(long id);
        Task<List<OrderItemViewModel>> GetItemsAsync(long orderId);
        Task<List<OrderViewModel>> SearchAsync(OrderSearchCriteria searchModel);
        Task<long> FinalizeFromCartAsync(long cartId, string transactionId);
    }
}
