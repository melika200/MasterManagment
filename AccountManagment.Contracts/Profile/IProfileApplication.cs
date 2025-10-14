using _01_FrameWork.Application;

namespace AccountManagment.Application.Contracts.Profile;

public interface IProfileApplication
{
    Task<OperationResult> CreateProfileAsync(CreateProfileCommand command);
    Task<OperationResult> EditProfileAsync(EditProfileCommand command);
    Task<ProfileViewModel?> GetProfileByUserIdAsync(long userId);
    Task<IEnumerable<ProfileViewModel>> GetAllProfileAsync();
    Task<ProfileViewModel?> GetProfileByIdAsync(long id);
    Task<OperationResult> DeleteProfileAsync(long id);
}
