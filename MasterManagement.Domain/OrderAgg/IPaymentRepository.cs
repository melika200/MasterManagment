using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;

namespace MasterManagement.Domain.OrderAgg
{
    public interface IPaymentRepository : IRepository<long, Payment>
    {
        Task<Payment?> GetByTransactionIdAsync(string transactionId);
        Task<List<Payment>> GetPaymentsByOrderIdAsync(long orderId);
    }
}
