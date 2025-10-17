namespace MasterManagment.Application.Contracts.Support;

public class ChangeSupportStatusCommand
{
    public long Id { get; set; }
    public string Status { get; set; } = string.Empty;
}
