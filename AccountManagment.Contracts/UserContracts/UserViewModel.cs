namespace AccountManagment.Contracts.UserContracts;

public class UserViewModel
{
    public long Id { get; set; }
    public string? Fullname { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    //public string? Mobile { get; set; }
    public string? Role { get; set; }
}
