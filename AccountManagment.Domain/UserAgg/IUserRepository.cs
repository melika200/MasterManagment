using _01_FrameWork.Domain;

namespace AccountManagment.Domain.UserAgg;

public interface IUserRepository : IRepository<long, User>
{
    //IQueryable<User> GetAccountsByIds(List<long> accountIds);

    //EditUserViewModel? GetForEdit(long id);
    //List<UserViewModel>? Search(UserSearchCriteria criteria);
    Task<User?> GetUserWithRoleAsync(string username);
    Task<List<User>> GetAllUsersAsync();
   

}
