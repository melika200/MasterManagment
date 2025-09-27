using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Order
{
    public class CartItemViewModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Count { get; set; }
        public double UnitPrice { get; set; }
        public int DiscountRate { get; set; }
    }
}
