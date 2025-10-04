using _01_FrameWork.Infrastructure;
using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagment.Domain.UserAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repository;

public class UserRepository : RepositoryBase<long, User>, IUserRepository
{
    private readonly AccountContext _context;

    public UserRepository(AccountContext context) : base(context)
    {
        _context = context;
    }

    public Task<User?> GetUserWithRoleAsync(string username)
    {
        return _context.Users.Where(x => x.Username == username)
            .Include(x => x.Role)
            .FirstOrDefaultAsync();
    }
}
