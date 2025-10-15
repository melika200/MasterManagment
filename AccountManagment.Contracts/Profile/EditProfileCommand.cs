using Microsoft.AspNetCore.Http;

namespace AccountManagment.Application.Contracts.Profile;

public class EditProfileCommand
{
   
    //public long Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public IFormFile? AvatarFile { get; set; }
}
