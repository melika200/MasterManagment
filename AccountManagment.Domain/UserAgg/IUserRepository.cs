using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;

namespace AccountManagment.Domain.UserAgg
{
    public interface IUserRepository : IRepository<long,User>
    {
        //IQueryable<User> GetAccountsByIds(List<long> accountIds);

        //EditUserViewModel? GetForEdit(long id);
        //List<UserViewModel>? Search(UserSearchCriteria criteria);
    }
}
