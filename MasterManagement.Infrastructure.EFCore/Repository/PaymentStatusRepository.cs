using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.PaymentStatusAgg;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository;

public class PaymentStatusRepository : RepositoryBase<long, PaymentStatus>, IPaymentStatusRepository
{
    public PaymentStatusRepository(DbContext context) : base(context)
    {
    }
}
