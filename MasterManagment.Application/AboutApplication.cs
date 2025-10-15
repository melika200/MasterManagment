using _01_FrameWork.Application.Exceptions;
using MasterManagement.Domain.AboutUs.Agg;
using MasterManagement.Domain.AboutUsAgg;
using MasterManagment.Application.Contracts.AboutUs;

namespace MasterManagment.Application;

public class AboutApplication : IAboutApplication
{
    private readonly IAboutRepository _aboutRepository;

    public AboutApplication(IAboutRepository repository)
    {
        _aboutRepository = repository;
    }

    public async Task Create(CreateAboutCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            throw new ArgumentException("Title cannot be empty.", nameof(command.Title));

        if (string.IsNullOrWhiteSpace(command.Description))
            throw new ArgumentException("Description cannot be empty.", nameof(command.Description));

        var about = new About(command.Title, command.Description);
        await _aboutRepository.CreateAsync(about);
    }

    public async Task Edit(EditAboutCommand command)
    {
        var about = await _aboutRepository.GetAsync(command.Id);
        if (about == null)
            throw new EntityNotFoundException(nameof(About), command.Id);

        if (string.IsNullOrWhiteSpace(command.Title))
            throw new ArgumentException("Title cannot be empty.", nameof(command.Title));

        if (string.IsNullOrWhiteSpace(command.Description))
            throw new ArgumentException("Description cannot be empty.", nameof(command.Description));

        about.Update(command.Title, command.Description);
        await _aboutRepository.UpdateAsync(about);
    }

    public async Task<AboutViewModel?> GetActiveAbout()
    {
        var about = await _aboutRepository.GetActiveAbout();
        if (about == null) return null;

        return new AboutViewModel
        {
            Id = about.Id,
            Title = about.Title,
            Description = about.Description
        };
    }
}
