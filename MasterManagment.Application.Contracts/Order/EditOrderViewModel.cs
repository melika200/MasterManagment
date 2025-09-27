using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Order
{
    public class EditOrderViewModel
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public int PaymentMethod { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double PayAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCanceled { get; set; }
        public string? IssueTrackingNo { get; set; }
        public List<OrderItemViewModel> Items { get; set; }

        public EditOrderViewModel()
        {
            Items = new List<OrderItemViewModel>();
        }
    }
}
