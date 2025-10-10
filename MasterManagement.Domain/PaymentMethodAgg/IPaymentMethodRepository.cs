using _01_FrameWork.Domain;

namespace MasterManagement.Domain.PaymentMethodAgg;

public interface IPaymentMethodRepository : IRepository<long, PaymentMethod>
{
}
