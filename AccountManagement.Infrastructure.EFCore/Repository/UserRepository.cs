using _01_FrameWork.Infrastructure;
using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagment.Domain.RefreshTokenAgg;
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

    public async Task<List<User>> GetAllUsersAsync()
    {

        return await _context.Users
            .Include(u => u.Role)
            .ToListAsync();
    }

    public Task<User?> GetUserWithRoleAsync(string username)
    {
        return _context.Users.Where(x => x.Username == username)
            .Include(x => x.Role)
            .FirstOrDefaultAsync();
    }
    public Task<User?> GetUserWithRoleByIdForRefreshTokenAsync(long userId)
    {
        return _context.Users
            .Where(x => x.Id == userId && !x.IsDeleted)
            .Include(x => x.Role)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Refresh Token and Logout
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == token && !r.IsDeleted && !r.IsRevoked);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        if (refreshToken == null)
            return false;

        _context.RefreshTokens.Remove(refreshToken);
        return true;
    }
    public void AddRefreshToken(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
    }



}
