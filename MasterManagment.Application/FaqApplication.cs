using _01_FrameWork.Application.Exceptions;
using MasterManagement.Domain.FaqUs.Agg;
using MasterManagement.Domain.FaqUsAgg;
using MasterManagment.Application.Contracts.FaqUs;
using MasterManagment.Application.Contracts.UnitOfWork;

namespace MasterManagment.Application;

public class FaqApplication : IFaqApplication
{
    private readonly IFaqRepository _faqRepository;
    private readonly IMasterUnitOfWork _unitOfWork;
    public FaqApplication(IFaqRepository repository, IMasterUnitOfWork unitOfWork )
    {
        _faqRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Create(CreateFaqCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Question))
            throw new ArgumentException("Question cannot be empty.", nameof(command.Question));

        if (string.IsNullOrWhiteSpace(command.Answer))
            throw new ArgumentException("Answer cannot be empty.", nameof(command.Answer));

        var faq = new Faq(command.Question, command.Answer);
        await _faqRepository.CreateAsync(faq);
        await _unitOfWork.CommitAsync();
    }

    public async Task Edit(EditFaqCommand command)
    {
        var faq = await _faqRepository.GetAsync(command.Id);
        if (faq == null)
            throw new EntityNotFoundException(nameof(Faq), command.Id);

        if (string.IsNullOrWhiteSpace(command.Question))
            throw new ArgumentException("Question cannot be empty.", nameof(command.Question));

        if (string.IsNullOrWhiteSpace(command.Answer))
            throw new ArgumentException("Answer cannot be empty.", nameof(command.Answer));

        faq.Update(command.Question, command.Answer);
        await _faqRepository.UpdateAsync(faq);
        await _unitOfWork.CommitAsync();
    }

    public async Task<FaqViewModel?> GetById(long id)
    {
        var faq = await _faqRepository.GetAsync(id);
        if (faq == null) return null;

        return new FaqViewModel
        {
            Id = faq.Id,
            Question = faq.Question,
            Answer = faq.Answer
        };
    }

    public async Task<List<FaqViewModel>> Search(SearchFaqCriteria criteria)
    {
        var faqs = await _faqRepository.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(criteria.Question))
            faqs = faqs.Where(f => f.Question.Contains(criteria.Question)).ToList();

        if (criteria.IsActive.HasValue)
            faqs = faqs.Where(f => f.IsActive == criteria.IsActive.Value).ToList();

        return faqs.Select(f => new FaqViewModel
        {
            Id = f.Id,
            Question = f.Question,
            Answer = f.Answer
        }).ToList();
    }
}
