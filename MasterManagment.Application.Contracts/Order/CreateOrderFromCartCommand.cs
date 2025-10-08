using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.OrderContracts
{
    
    
        public class CreateOrderFromCartCommand
        {
            public long CartId { get; set; }
            public string? TransactionId { get; set; }
        }
    

}
