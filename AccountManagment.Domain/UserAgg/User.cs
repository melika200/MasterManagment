using _01_FrameWork.Domain;

namespace AccountManagment.Domain.UserAgg;

public class User:EntityBase,ISoftDelete
{
    public bool IsDeleted { get; set; } = false;

    public string Username { get; private set; }
    public string? Fullname { get; private set; }
    public string? Password { get; private set; }
    public string? Mobile { get; private set; }
    public int RoleId { get; private set; }

    public RoleAgg.Role Role { get; private set; }


    public User(string? fullname, string username, string? password, string? mobile, RoleAgg.Role role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role), "Role cannot be null in User constructor.");
        Fullname = fullname;
        Username = username;
        Password = password;
        Mobile = mobile;
        Role = role;
        RoleId = role.Id;
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
        Mobile = mobile;
      
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
