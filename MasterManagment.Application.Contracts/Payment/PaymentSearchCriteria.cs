using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Payment
{
    public class PaymentSearchCriteria
    {
        public long CartId { get; set; }
        public bool? IsSucceeded { get; set; }
        public string? TransactionId { get; set; }
    }
}

  

    

   

    

   

