using _01_FrameWork.Domain;

namespace DiscountManagment.Domain.DiscountAgg;

public class Discount : EntityBase
{
    public string Title { get; private set; } = string.Empty;
    public int Rate { get; private set; }
    public decimal? Amount { get; private set; }
    public bool IsInPercent { get; private set; } = true;
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Reason { get; private set; }
    public bool IsActive { get; private set; } = true;

    protected Discount() { }

    public Discount(string title, int rate, bool isInPercent, decimal? amount,
                    DateTime? startDate, DateTime? endDate, string? reason)
    {
        Title = title;
        Rate = rate;
        IsInPercent = isInPercent;
        Amount = amount;
        StartDate = startDate;
        EndDate = endDate;
        Reason = reason;
    }

    public void Edit(string title, int rate, bool isInPercent, decimal? amount,
                     DateTime? startDate, DateTime? endDate, string? reason)
    {
        Title = title;
        Rate = rate;
        IsInPercent = isInPercent;
        Amount = amount;
        StartDate = startDate;
        EndDate = endDate;
        Reason = reason;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
