using _01_FrameWork.Domain;
using AccountManagment.Domain.UserAgg;

namespace AccountManagment.Domain.ProfileAgg;

public class Profile : EntityBase, ISoftDelete
{
    public long UserId { get; private set; }
    public string? FullName { get; private set; }
    public string? Email { get; private set; }
    public string? Address { get; private set; }
    public string? PostalCode { get; private set; }
    public string? AvatarPath { get; private set; }
    public bool IsDeleted { get; set; } = false;
    public User? User { get; private set; }

    protected Profile() { }

    public Profile(long userId, string? fullName, string? email, string? address, string? postalCode, string? avatarPath)
    {
        UserId = userId;
        FullName = fullName;
        Email = email;
        Address = address;
        PostalCode = postalCode;
        AvatarPath = avatarPath;
        IsDeleted = false;
    }

    public void Edit(string? fullName, string? email, string? address, string? postalCode, string? avatarPath)
    {
        FullName = fullName;
        Email = email;
        Address = address;
        PostalCode = postalCode;

        if (!string.IsNullOrWhiteSpace(avatarPath))
            AvatarPath = avatarPath;
    }

    public void SoftDelete() => IsDeleted = true;
}
