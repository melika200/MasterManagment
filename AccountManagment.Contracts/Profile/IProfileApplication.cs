using System.Security.Claims;
using _01_FrameWork.Application;

namespace AccountManagment.Application.Contracts.Profile;

public interface IProfileApplication
{
    Task<OperationResult> CreateProfileAsync(CreateProfileCommand commandو, ClaimsPrincipal user);
    Task<OperationResult> EditProfileAsync(EditProfileCommand commandو, ClaimsPrincipal user);
    Task<ProfileViewModel?> GetProfileByUserIdAsync(ClaimsPrincipal user);
    Task<ProfileViewModel?> GetProfileByUserIdInAdminAsync(long userId);
    Task<IEnumerable<ProfileViewModel>> GetAllProfileAsync();
    Task<ProfileViewModel?> GetProfileByIdAsync(long id);
    Task<OperationResult> DeleteProfileAsync(long id);
}
