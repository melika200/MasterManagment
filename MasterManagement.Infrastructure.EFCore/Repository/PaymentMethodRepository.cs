using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.PaymentMethodAgg;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository;

class PaymentMethodRepository : RepositoryBase<long, PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(DbContext context) : base(context)
    {
    }
}
