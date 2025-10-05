namespace AccountManagment.Contracts.UserContracts;

public class EditUserViewModel
{
    public long Id { get; set; }
    public string? Fullname { get; set; }
    public string? Username { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PostalCode { get; set; }
    public int RoleId { get; set; }

    public List<Domain.RoleAgg.Role> AllRoles { get; set; } = new();
}

