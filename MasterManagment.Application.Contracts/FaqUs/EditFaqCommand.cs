namespace MasterManagment.Application.Contracts.FaqUs;

public class EditFaqCommand
{
    public long Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}
