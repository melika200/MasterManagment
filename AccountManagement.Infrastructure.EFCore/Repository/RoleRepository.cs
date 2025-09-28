using _01_FrameWork.Infrastructure;
using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagment.Domain.Role;
using AccountManagment.Domain.RoleAgg;

namespace AccountManagement.Infrastructure.EFCore.Repository;

public class RoleRepository : RepositoryBase<long, Role>, IRoleRepository
{
    private readonly AccountContext _Context;
    public RoleRepository(AccountContext context) : base(context)
    {
        _Context = context;
    }
}
