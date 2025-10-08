using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.OrderContracts
{
    public class EditOrderCommand : CreateOrderCommand
    {
        public long Id { get; set; }
    }
}
