using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.GalleryAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Infrastructure.EFCore.Context;

namespace MasterManagement.Infrastructure.EFCore.Repository;

public class GalleryRepository : RepositoryBase<long, Gallery>, IGalleryRepository
{
    private readonly MasterContext _context;

    public GalleryRepository(MasterContext context) : base(context)
    {
        _context = context;
    }
}
