using _01_FrameWork.Domain;
using MasterManagement.Domain.PaymentAgg;

namespace MasterManagement.Domain.PaymentStatusAgg;

public class PaymentStatus : Enumeration
{
    public PaymentStatus(int id, string name) : base(id, name) { }

    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    private PaymentStatus() : base() { }
}
