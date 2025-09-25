using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Payment
{
    public class CreatePaymentCommand
    {
        public long CartId { get; set; }
        public double Amount { get; set; }
        public string TransactionId { get; set; }
        public bool IsSucceeded { get; set; }
    }
}
