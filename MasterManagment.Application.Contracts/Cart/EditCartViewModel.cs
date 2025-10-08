using MasterManagment.Application.Contracts.CartItem;

namespace MasterManagment.Application.Contracts.OrderContracts;


public class EditCartViewModel
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
    public List<CartItemViewModel> Items { get; set; }

    public EditCartViewModel()
    {
        Items = new List<CartItemViewModel>();
    }
}


