using _01_FrameWork.Domain;
using AccountManagment.Domain.UserAgg;

namespace AccountManagment.Domain.RoleAgg;

public class Role:ISoftDelete
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public bool IsDeleted { get; set; } = false;


    public ICollection<User> Users { get; private set; } = new List<User>();


    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
