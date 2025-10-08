using MasterManagment.Application.Contracts.CartItem;

namespace MasterManagment.Application.Contracts.OrderContracts
{
    public class CreateCartCommand
    {
        public long AccountId { get; set; }
        //public string? AccountName { get; set; }
        public int PaymentMethod { get; set; }
        public List<CartItemViewModel>? Items { get; set; }
        //public double DiscountAmount { get; set; }
        //public double PayAmount { get; set; }
        //public double TotalAmount { get; set; }
    }



}
