using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.OrderStateAgg;
using MasterManagement.Infrastructure.EFCore.Context;

namespace MasterManagement.Infrastructure.EFCore.Repository;

class OrderStateRepository : RepositoryBase<long, OrderState>, IOrderStateRepository
{
    private readonly MasterContext _Context;
    public OrderStateRepository(MasterContext context) : base(context)
    {
        _Context = context;
    }
}
