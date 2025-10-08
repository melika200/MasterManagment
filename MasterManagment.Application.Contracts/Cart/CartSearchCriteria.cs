namespace MasterManagment.Application.Contracts.OrderContracts;

public class CartSearchCriteria
{
    public long AccountId { get; set; }
    public bool? IsPaid { get; set; }
    public bool? IsCanceled { get; set; }
}
