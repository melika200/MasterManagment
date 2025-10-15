namespace MasterManagment.Application.Contracts.FaqUs;

public class CreateFaqCommand
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}