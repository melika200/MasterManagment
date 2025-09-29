using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;

namespace AccountManagment.Domain.UserAgg
{
    public interface IUserRepository : IRepository<long,User>
    {
        Task<User?> GetSingleAsync(Expression<Func<User, bool>> predicate);

        //IQueryable<User> GetAccountsByIds(List<long> accountIds);

        //EditUserViewModel? GetForEdit(long id);
        //List<UserViewModel>? Search(UserSearchCriteria criteria);
    }
}
