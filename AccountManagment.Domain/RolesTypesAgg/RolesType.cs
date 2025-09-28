namespace AccountManagment.Domain.RolesTypesAgg;

public static class RolesType
{
    public static readonly RoleAgg.Role User = new RoleAgg.Role(1, "User");
    public static readonly RoleAgg.Role Admin = new RoleAgg.Role(2, "Admin");
    public static readonly RoleAgg.Role Programmer = new RoleAgg.Role(3, "Programmer");

    public static readonly List<RoleAgg.Role> AllTypes = new()
    {
        User,
        Admin,
        Programmer
    };
}
