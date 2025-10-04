using _01_FrameWork.Application;

namespace AccountManagment.Contracts.UserContracts;

public interface IUserApplication
{
    Task<OperationResult> Create(CreateUserCommand command);
    Task<Domain.UserAgg.User?> GetUserWithRoleByUsernameAsync(string username);
     //
    Task<OperationResult> Edit(EditUserCommand command);
    Task<OperationResult> ChangePassword(ChangePasswordCommand command);
    EditUserViewModel? GetForEdit(long id);

    Task<OperationResult> DeleteAsync(long id);
    Task<OperationResult> ActivateAsync(long id);
    Task<OperationResult> DeactivateAsync(long id);
    Task<OperationResult> ChangeRoleAsync(ChangeUserRoleCommand command);
    List<UserViewModel>? Search(UserSearchCriteria criteria);
    //bool IsAccountExists(CreateUserCommand command);
    Task<bool> IsExistsBy(string username);
    long GetUserId(string? username);
    //List<UserViewModel> GetAccountsByIds(List<long> accountIds);
}
