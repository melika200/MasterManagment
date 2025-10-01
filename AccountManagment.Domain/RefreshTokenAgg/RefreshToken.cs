using _01_FrameWork.Domain;
using AccountManagment.Domain.UserAgg;

namespace AccountManagment.Domain.RefreshTokenAgg;

public class RefreshToken:ISoftDelete
{
    public Guid Id { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public bool IsRevoked { get; private set; }
    public bool IsDeleted { get; set; } = false;
    public long UserId { get; private set; }
    public User? User { get; private set; }

    public RefreshToken(string token, DateTime expiryDate, long userId)
    {
        Id = Guid.NewGuid();
        Token = token;
        ExpiryDate = expiryDate;
        UserId = userId;
        IsRevoked = false;
    }

    public void Revoke() => IsRevoked = true;

    public bool IsExpired() => DateTime.UtcNow >= ExpiryDate;
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
