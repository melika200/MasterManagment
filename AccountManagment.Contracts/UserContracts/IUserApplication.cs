using _01_FrameWork.Application;

namespace AccountManagment.Contracts.UserContracts;

public interface IUserApplication
{
    Task<OperationResult> Create(CreateUserCommand command);
    Task<Domain.UserAgg.User?> GetUserWithRoleByUsernameAsync(string username);
     //
    Task<OperationResult> Edit(EditUserCommand command);
    Task<OperationResult> ChangePassword(ChangePasswordCommand command);
    Task<EditUserViewModel?> GetForEdit(long id);
    Task<List<UserViewModel>> GetAccountsByIds(List<long> accountIds);
    Task<OperationResult> DeleteAsync(long id);
    Task<OperationResult> ActivateAsync(long id);
    Task<OperationResult> DeactivateAsync(long id);
    Task<OperationResult> ChangeRoleAsync(long userId, ChangeUserRoleCommand command);
    Task<List<UserViewModel>> GetAllUsers();
    //List<UserViewModel>? Search(UserSearchCriteria criteria);
    //bool IsAccountExists(CreateUserCommand command);
    Task<bool> IsExistsBy(string username);
    long GetUserId(string? username);
    //List<UserViewModel> GetAccountsByIds(List<long> accountIds);
}
