using _01_FrameWork.Domain;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.ProductAgg;
using System;

public class CartItem : EntityBase
{
    public long ProductId { get; private set; }
    public Product? Product { get; private set; } 
    public long CartId { get; private set; }
    public Cart? Cart { get; private set; } 
    public string ProductName { get; private set; } = string.Empty;
    public int Count { get; private set; }
    public double UnitPrice { get; private set; }
    public int DiscountRate { get; private set; }


    protected CartItem() { }

    public CartItem(long productId, string productName, double unitPrice, int count, int discountRate)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Count = count;
        DiscountRate = discountRate;
    }

    internal void SetCart(Cart cart)
    {
        Cart = cart ?? throw new ArgumentNullException(nameof(cart));
        CartId = cart.Id;
    }

    public void IncreaseCount(int amount)
    {
        if (amount > 0)
            Count += amount;
    }

    public double GetTotalPrice()
    {
        return UnitPrice * Count;
    }

    public double GetDiscountAmount()
    {
        return GetTotalPrice() * DiscountRate / 100.0;
    }
}
