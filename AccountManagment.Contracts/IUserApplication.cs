using _01_FrameWork.Application;
using AccountManagment.Domain.UserAgg;

namespace AccountManagment.Contracts;

public interface IUserApplication
{
    Task<OperationResult> Create(CreateUserCommand command);
    Task<User?> GetByUsernameAsync(string username);
     //
    Task<OperationResult> Edit(EditUserCommand command);
    Task<OperationResult> ChangePassword(ChangePasswordCommand command);
    EditUserViewModel? GetForEdit(long id);
    List<UserViewModel>? Search(UserSearchCriteria criteria);
    //bool IsAccountExists(CreateUserCommand command);
    Task<bool> IsExistsBy(string username);
    long GetUserId(string? username);
    //List<UserViewModel> GetAccountsByIds(List<long> accountIds);
}
