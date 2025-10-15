namespace MasterManagment.Application.Contracts.FaqUs;

public interface IFaqApplication
{
    Task Create(CreateFaqCommand command);
    Task Edit(EditFaqCommand command);
    Task<FaqViewModel?> GetById(long id);
    Task<List<FaqViewModel>> Search(SearchFaqCriteria criteria);
}
