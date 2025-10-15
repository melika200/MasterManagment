using _01_FrameWork.Domain;
using MasterManagement.Domain.FaqUsAgg;

namespace MasterManagement.Domain.FaqUs.Agg;

public interface IFaqRepository : IRepository<long, Faq>
{
    Task<List<Faq>> GetActiveFaqs();
}
