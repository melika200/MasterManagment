using _01_FrameWork.Domain;
using MasterManagement.Domain.AboutUsAgg;

namespace MasterManagement.Domain.AboutUs.Agg;

public interface IAboutRepository : IRepository<long, About>
{
    Task<About?> GetActiveAbout();
}
