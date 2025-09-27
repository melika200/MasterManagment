using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Order
{
    public class OrderViewModel
    {
        public long Id { get; set; }
        public string? AccountName { get; set; }
        public int PaymentMethod { get; set; }
        public double TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCanceled { get; set; }
        public string? IssueTrackingNo { get; set; }
    }
}




