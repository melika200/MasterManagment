using _01_FrameWork.Domain;
using MasterManagement.Domain.PaymentMethodAgg;

namespace MasterManagement.Domain.CartAgg;



//نماینده یک سفارش کلی مشتری است که کالاها را به صورت یک مجموعه خریداری می‌کند.
//شامل شناسه حساب کاربری مشتری، روش پرداخت، مبالغ کلی، وضعیت پرداخت(آیا پرداخت شده؟)، وضعیت لغو(کنسل شده؟)، شماره پیگیری ارسال کالا  و شناسه تراکنش پرداخت است.
public class Cart : EntityBase,ISoftDelete
{
    public long AccountId { get; private set; }
    //public string AccountName { get; private set; }
    public bool IsDeleted { get; set; } = false;
    public int PaymentMethodId { get; private set; }
    public string PaymentMethodName { get; private set; }
    public double TotalAmount { get; private set; }
    public double DiscountAmount { get; private set; }
    public double PayAmount { get; private set; }
    public bool IsPaid { get; private set; }
    public bool IsCanceled { get; private set; }
    public string IssueTrackingNo { get; private set; }
    public string RefId { get; private set; }

    public List<CartItem> Items { get; private set; }

    public Cart(long accountId, int paymentMethodId, string paymentMethodName, double totalAmount, double discountAmount, double payAmount)
    {
        AccountId = accountId;
        //AccountName = accountName;
        PaymentMethodId = paymentMethodId;
        PaymentMethodName = paymentMethodName;
        TotalAmount = totalAmount;
        DiscountAmount = discountAmount;
        PayAmount = payAmount;
        IssueTrackingNo = string.Empty;
        IsPaid = false;
        IsCanceled = false;
        RefId = string.Empty;
        Items = new List<CartItem>();
    }

    public void AddItem(CartItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        item.SetCart(this);
        var existing = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existing != null)
        {
            existing.IncreaseCount(item.Count);
        }
        else
        {
            Items.Add(item);
        }
        RecalculateAmounts();
    }

    public void RemoveItem(long productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateAmounts();
        }
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
    public void Edit(int paymentMethodId, string paymentMethodName, double totalAmount, double discountAmount, double payAmount)
    {
        PaymentMethodId = paymentMethodId;
        PaymentMethodName = paymentMethodName;
        TotalAmount = totalAmount;
        DiscountAmount = discountAmount;
        PayAmount = payAmount;
    }

    public void PaymentSucceeded(string refId)
    {
        IsPaid = true;
        if (refId != null) RefId = refId;
    }
    public void SetPaymentMethod(int methodId, string methodName)
    {
        PaymentMethodId = methodId;
        PaymentMethodName = methodName;
    }
    public void Cancel()
    {
        IsCanceled = true;
    }

    public void SetIssueTrackingNo(string number)
    {
        IssueTrackingNo = number;
    }

    private void RecalculateAmounts()
    {
        TotalAmount = Items.Sum(i => i.GetTotalPrice());
        DiscountAmount = Items.Sum(i => i.GetDiscountAmount());
        PayAmount = TotalAmount - DiscountAmount;
    }

    public void ClearItems()
    {
        foreach (var item in Items.ToList())
        {
            Items.Remove(item);
        }
        RecalculateAmounts();
    }
}

