using _01_FrameWork.Domain;
using DiscountManagement.Application.Contracts.Discount;

namespace DiscountManagement.Domain.DiscountAgg;

public interface IDiscountRepository : IRepository<long, Discount>
{
    Task<EditDiscountViewModel?> GetForEditAsync(long id);
    Task<IEnumerable<DiscountViewModel>> SearchAsync(SearchDiscountCriteria criteria);
}
