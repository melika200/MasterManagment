using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;
using MasterManagment.Application.Contracts.Order;

namespace MasterManagement.Domain.OrderAgg
{
    public interface IOrderRepository : IRepository<long, Order>
    {
        Task<Order?> GetOrderDetailsAsync(long orderId);
        Task<List<Order>> SearchAsync(OrderSearchCriteria criteria);
    }
}
