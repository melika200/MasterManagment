namespace MasterManagment.Application.Contracts.OrderContracts;

public class EditCartCommand : CreateCartCommand
{
    public long Id { get; set; }
}
