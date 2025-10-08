namespace MasterManagment.Application.Contracts.CartItem;

public class CartItemViewModel
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Count { get; set; }
    //public double UnitPrice { get; set; }
    public int DiscountRate { get; set; }
}
