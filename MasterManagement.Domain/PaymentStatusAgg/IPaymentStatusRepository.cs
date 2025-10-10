using _01_FrameWork.Domain;

namespace MasterManagement.Domain.PaymentStatusAgg;

public interface IPaymentStatusRepository : IRepository<long, PaymentStatus>
{
}
