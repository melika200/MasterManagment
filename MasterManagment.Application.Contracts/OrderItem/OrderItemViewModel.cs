using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.OrderItem
{
    public class OrderItemViewModel
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }
        public int DiscountRate { get; set; }
        public long AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountPhone { get; set; }
        public string? AccountAddress { get; set; }
        public int PaymentMethod { get; set; }
        public double TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCanceled { get; set; }
        public string? IssueTrackingNo { get; set; }
        public List<OrderItemViewModel>? Items { get; set; }


    }
}
