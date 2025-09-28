using _01_FrameWork.Infrastructure;
using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagment.Domain.UserAgg;

namespace AccountManagement.Infrastructure.EFCore.Repository;

public class UserRepository : RepositoryBase<long, User>, IUserRepository
{
    private readonly AccountContext _Context;
    public UserRepository(AccountContext context):base(context)
    {
        _Context = context;
    }

  
}
