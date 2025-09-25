using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Order
{
    public class CreateCartCommand
    {
        public long AccountId { get; set; }
        public int PaymentMethod { get; set; }
        public List<CartItemDto> Items { get; set; }
        //public double DiscountAmount { get; set; }
        //public double PayAmount { get; set; }
        //public double TotalAmount { get; set; }
    }



}
