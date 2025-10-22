using _01_FrameWork.Domain;
using AccountManagment.Domain.RefreshTokenAgg;

namespace AccountManagment.Domain.UserAgg;

public interface IUserRepository : IRepository<long, User>
{
    //IQueryable<User> GetAccountsByIds(List<long> accountIds);

    //EditUserViewModel? GetForEdit(long id);
    //List<UserViewModel>? Search(UserSearchCriteria criteria);
    Task<User?> GetUserWithRoleAsync(string username);
    Task<bool> RevokeRefreshTokenAsync(string token);
    Task<List<User>> GetAllUsersAsync();
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<User?> GetUserWithRoleByIdForRefreshTokenAsync(long userId);
    void AddRefreshToken(RefreshToken token);

}
