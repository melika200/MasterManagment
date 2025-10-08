using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.ShippingStatusAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository;

class ShippingStatusRepository : RepositoryBase<long, ShippingStatus>, IShippingStatusRepository
{
    private readonly MasterContext _Context;
    public ShippingStatusRepository(MasterContext context) : base(context)
    {
        _Context = context;
    }
}
