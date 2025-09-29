using System.Linq.Expressions;
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

    public async Task<User?> GetSingleAsync(Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.SingleOrDefaultAsync(predicate);
    }
}
