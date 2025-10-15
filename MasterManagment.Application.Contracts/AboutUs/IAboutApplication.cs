namespace MasterManagment.Application.Contracts.AboutUs;

public interface IAboutApplication
{
    Task Create(CreateAboutCommand command);
    Task Edit(EditAboutCommand command);
    Task<AboutViewModel?> GetActiveAbout();
}
