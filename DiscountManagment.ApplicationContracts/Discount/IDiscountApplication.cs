using _01_FrameWork.Application;

namespace DiscountManagement.Application.Contracts.Discount;

public interface IDiscountApplication
{
    Task<OperationResult> CreateAsync(CreateDiscountCommand command);
    Task<OperationResult> EditAsync(EditDiscountCommand command);
    Task<EditDiscountViewModel?> GetForEditAsync(long id);
    Task<IEnumerable<DiscountViewModel>> SearchAsync(SearchDiscountCriteria criteria);
}
