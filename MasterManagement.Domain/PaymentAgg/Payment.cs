using _01_FrameWork.Domain;
using MasterManagement.Domain.PaymentMethodAgg;
using MasterManagement.Domain.PaymentStatusAgg;
using MasterManagement.Domain.PaymentStatusesTypeAgg;

namespace MasterManagement.Domain.PaymentAgg;

public class Payment : EntityBase
{
    public long CartId { get; private set; }        
    public long OrderId { get; private set; }         
    public double Amount { get; private set; }
    public int PaymentMethodId { get; private set; }
    public PaymentMethod? PaymentMethod { get; private set; }
    public int PaymentStatusId { get; private set; }
    public PaymentStatus? Status { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public string TransactionId { get; private set; }
    public bool IsSucceeded { get; private set; }
    public bool IsCanceled { get; private set; }

    public Payment(long cartId, double amount, string transactionId, bool isSucceeded)
    {
        CartId = cartId;
        OrderId = 0;      
        Amount = amount;
        PaymentDate = DateTime.Now;
        TransactionId = transactionId;
        IsSucceeded = isSucceeded;
        IsCanceled = false;
    }

    public void Cancel()
    {
        IsCanceled = true;
    }

    public void Update(long cartId, double amount, string transactionId, bool isSucceeded)
    {
        CartId = cartId;
        Amount = amount;
        TransactionId = transactionId;
        IsSucceeded = isSucceeded;
    }

    public void MarkAsSucceeded(string transactionId)
    {
        IsSucceeded = true;
        TransactionId = transactionId;
        PaymentDate = DateTime.Now;
    }

    public void SetAsSucceeded()
    {
        IsSucceeded = true;
    }

    public void MarkAsFailed()
    {
        IsSucceeded = false;
    }

    public void SetOrderId(long orderId)
    {
        if (orderId != 0)
            OrderId = orderId;
    }
    public void MarkAsConfirmed()
    {
        PaymentStatusId = PaymentStatusesType.Confirmed.Id;
    }


}
