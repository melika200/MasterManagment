namespace MasterManagment.Application.Contracts.Payment;

public class ConfirmPaymentCommand
{
    public long PaymentId { get; set; }
    public long CartId { get; set; }
    public long AccountId { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }
    public double PayAmount { get; set; }
    //public string RefId { get; set; } 

}
