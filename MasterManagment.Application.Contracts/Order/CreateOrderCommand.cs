using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Order
{
    public class CreateOrderCommand
    {
        public long AccountId { get; set; }
        public int PaymentMethod { get; set; }
        public List<OrderItemDto>? Items { get; set; }
        public double DiscountAmount { get; set; }
        public double PayAmount { get; set; }
        public double TotalAmount { get; set; }
    }
}
