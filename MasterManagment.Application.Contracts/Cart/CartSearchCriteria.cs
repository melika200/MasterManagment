namespace MasterManagment.Application.Contracts.Order;

public class CartSearchCriteria
{
    public long AccountId { get; set; }
    public bool? IsPaid { get; set; }
    public bool? IsCanceled { get; set; }
}
