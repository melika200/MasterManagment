namespace MasterManagment.Application.Contracts.Order;

public class EditCartCommand : CreateCartCommand
{
    public long Id { get; set; }
}
