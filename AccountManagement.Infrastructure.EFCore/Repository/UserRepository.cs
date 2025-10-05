using System;
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

    public async Task<IEnumerable<User>> GetAllUsersAsync(Expression<Func<User, bool>> expression)
    {

        return await _context.Users
            .Include(u => u.Role)
            .Where(expression)
            .ToListAsync();
    }

    public Task<User?> GetUserWithRoleAsync(string username)
    {
        return _context.Users.Where(x => x.Username == username)
            .Include(x => x.Role)
            .FirstOrDefaultAsync();
    }
}
