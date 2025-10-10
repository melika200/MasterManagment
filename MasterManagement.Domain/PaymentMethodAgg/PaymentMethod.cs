using _01_FrameWork.Domain;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.PaymentAgg;

namespace MasterManagement.Domain.PaymentMethodAgg;

public class PaymentMethod : Enumeration
{
    public PaymentMethod(int id, string name) : base(id, name) { }

    public ICollection<Order> Orders { get; private set; } = new List<Order>();
    public ICollection<Cart> Carts { get; private set; } = new List<Cart>();
    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    private PaymentMethod() : base() { }
}
