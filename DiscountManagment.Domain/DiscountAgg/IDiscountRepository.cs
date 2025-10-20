using _01_FrameWork.Domain;
using DiscountManagment.Application.Contracts.Discount;


namespace DiscountManagment.Domain.DiscountAgg;

public interface IDiscountRepository : IRepository<long, Discount>
{
    Task<EditDiscountViewModel?> GetForEditAsync(long id);
    Task<IEnumerable<DiscountViewModel>> SearchAsync(SearchDiscountCriteria criteria);
}
