using _01_FrameWork.Application;
using DiscountManagement.Application.Contracts.Discount;
using DiscountManagement.Domain.DiscountAgg;

namespace DiscountManagement.Application;

public class DiscountApplication : IDiscountApplication
{
    private readonly IDiscountRepository _repository;

    public DiscountApplication(IDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult> CreateAsync(CreateDiscountCommand command)
    {
        var op = new OperationResult();

        var duplicate = await _repository.IsExistsAsync(x => x.Title == command.Title);
        if (duplicate)
            return op.Failed("تخفیف تکراری است");

        var discount = new Discount(command.Title, command.Rate, command.IsInPercent,
                                    command.Amount, command.StartDate, command.EndDate, command.Reason);

        await _repository.CreateAsync(discount);
        return op.Succedded();
    }

    public async Task<OperationResult> EditAsync(EditDiscountCommand command)
    {
        var op = new OperationResult();
        var discount = await _repository.GetAsync(command.Id);
        if (discount == null)
            return op.Failed("تخفیف یافت نشد");

        discount.Edit(command.Title, command.Rate, command.IsInPercent,
                      command.Amount, command.StartDate, command.EndDate, command.Reason);

        await _repository.UpdateAsync(discount);
        return op.Succedded();
    }

    public Task<EditDiscountViewModel?> GetForEditAsync(long id) => _repository.GetForEditAsync(id);

    public Task<IEnumerable<DiscountViewModel>> SearchAsync(SearchDiscountCriteria criteria)
        => _repository.SearchAsync(criteria);
}
