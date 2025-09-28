using _01_FrameWork.Domain;

namespace AccountManagment.Domain.Role;

public interface IRoleRepository : IRepository<long, RoleAgg.Role>
{
    //IQueryable<RoleEntities.Role> GetRolesByIds(List<long> roleIds);

 
}
