using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.OrderContracts
{
    public class OrderSearchCriteria
    {
        public long AccountId { get; set; }
        public long OrderId { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsCanceled { get; set; }
    }
}
