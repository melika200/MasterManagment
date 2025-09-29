using _01_FrameWork.Domain;
using AccountManagment.Domain.RoleAgg;
using AccountManagment.Domain.RolesTypesAgg;

namespace AccountManagment.Domain.UserAgg;

public class User:EntityBase,ISoftDelete
{
    public bool IsDeleted { get; set; } = false;

    public string Username { get; private set; }
    public string? Fullname { get; private set; }
    public string? Password { get; private set; }
    public int RoleId { get; private set; }

    public RoleAgg.Role? Role { get; private set; }


    public User(string username, int roleId)
    {
        Username = username;
        RoleId = roleId;
    }


    public void ChangeRole(RoleAgg.Role newRole)
    {
        if (newRole == null) throw new ArgumentNullException(nameof(newRole), "New role cannot be null.");
        Role = newRole;
        RoleId = newRole.Id;
    }


    public void Edit(string fullname, string username, string mobile)
    {
        Fullname = fullname;
        Username = username;
    }
  

    public void ChangePassword(string newPassword)
    {
        Password = newPassword;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
