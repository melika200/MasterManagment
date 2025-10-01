using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterManagment.Application.Contracts.OrderItem;

namespace MasterManagment.Application.Contracts.Order
{
    public class CreateOrderCommand
    {
        public long AccountId { get; set; }
        public int PaymentMethod { get; set; }
        public List<OrderItemViewModel>? Items { get; set; }
        public double DiscountAmount { get; set; }
        public double PayAmount { get; set; }
        public double TotalAmount { get; set; }
    }
}
