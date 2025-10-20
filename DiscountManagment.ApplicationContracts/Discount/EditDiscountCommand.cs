namespace DiscountManagement.Application.Contracts.Discount;

public class EditDiscountCommand : CreateDiscountCommand
{
    public long Id { get; set; }
}
