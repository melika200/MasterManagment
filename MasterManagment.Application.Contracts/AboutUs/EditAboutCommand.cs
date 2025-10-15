namespace MasterManagment.Application.Contracts.AboutUs;

public class EditAboutCommand
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}