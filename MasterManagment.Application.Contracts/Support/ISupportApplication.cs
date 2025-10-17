using _01_FrameWork.Application;

namespace MasterManagment.Application.Contracts.Support;

public interface ISupportApplication
{
    Task<OperationResult> CreateAsync(CreateSupportCommand command, long accountId);
    Task<OperationResult> EditAsync(EditSupportViewModel model);
    Task<OperationResult> DeleteAsync(long id);
    Task<OperationResult> ChangeStatusAsync(ChangeSupportStatusCommand command);

    Task<SupportViewModel?> GetDetails(long id);
    Task<List<SupportViewModel>> GetUserTickets(long accountId);
    Task<List<SupportViewModel>> SearchAsync(SupportSearchCriteria searchModel);
    Task<List<SupportViewModel>> GetAllAsync();
    Task<OperationResult> MarkAsRepliedAsync(long id);
}
