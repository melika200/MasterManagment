using _01_FrameWork.Domain;
using AccountManagment.Domain.ProfileAgg;
using AccountManagment.Domain.RefreshTokenAgg;

namespace AccountManagment.Domain.UserAgg;

public class User:EntityBase,ISoftDelete
{
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; private set; } = true;
    public string Username { get; private set; }
    public string? Fullname { get; private set; }
    public string? Password { get; private set; }
    public string? Address { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? PostalCode { get; private set; }
    public Profile? Profile { get; private set; }


    public int RoleId { get; private set; }

    public RoleAgg.Role? Role { get; private set; }
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();


    public User(string username, int roleId)
    {
        Username = username;
        RoleId = roleId;
        IsActive = true;
    }


    public void ChangeRole(RoleAgg.Role newRole)
    {
        if (newRole == null) throw new ArgumentNullException(nameof(newRole), "New role cannot be null.");
        Role = newRole;
        RoleId = newRole.Id;
    }


    public void Edit(string fullname, string username,string password)
    {
        Fullname = fullname;
        Username = username;
        Password = password;
    }
  

    public void ChangePassword(string newPassword)
    {
        Password = newPassword;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }

    public void Activate()
    {
        IsActive = true;
    }

   
    public void Deactivate()
    {
        IsActive = false;
    }
}
