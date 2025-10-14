namespace AccountManagment.Application.Contracts.Profile;

public class ProfileViewModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? AvatarPath { get; set; }
}
