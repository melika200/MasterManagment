using _01_FrameWork.Domain;

namespace MasterManagement.Domain.PaymentAgg;

public interface IPaymentRepository : IRepository<long, Payment>
{
    Task<Payment?> GetByTransactionIdAsync(string transactionId);
    Task<List<Payment>> GetPaymentsByOrderIdAsync(long orderId);
}
