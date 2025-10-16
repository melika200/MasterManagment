using _01_FrameWork.Application.Exceptions;
using MasterManagement.Domain.AboutUs.Agg;
using MasterManagement.Domain.AboutUsAgg;
using MasterManagment.Application.Contracts.AboutUs;
using MasterManagment.Application.Contracts.UnitOfWork;

namespace MasterManagment.Application;

public class AboutApplication : IAboutApplication
{
    private readonly IAboutRepository _aboutRepository;
    private readonly IMasterUnitOfWork _unitOfWork;

    public AboutApplication(IAboutRepository repository, IMasterUnitOfWork unitOfWork )
    {
        _aboutRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Create(CreateAboutCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            throw new ArgumentException("Title cannot be empty.", nameof(command.Title));

        if (string.IsNullOrWhiteSpace(command.Description))
            throw new ArgumentException("Description cannot be empty.", nameof(command.Description));

        var about = new About(command.Title, command.Description);
        await _aboutRepository.CreateAsync(about);
        await _unitOfWork.CommitAsync();
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
        await _unitOfWork.CommitAsync();
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

    public async Task<List<AboutViewModel>> Search(AboutSearchCriteria criteria)
    {
         
        var about = await _aboutRepository.GetAllAsync();

     
        if (criteria.IsActive.HasValue)
            about = about.Where(f => f.IsActive == criteria.IsActive.Value).ToList();

        return about.Select(a => new AboutViewModel
        {
            Id = a.Id,
             Description=a.Description,
             Title=a.Title
        }).ToList();
    
}
}
