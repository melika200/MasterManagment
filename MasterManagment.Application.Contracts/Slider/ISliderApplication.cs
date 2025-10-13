using _01_FrameWork.Application;
using MasterManagment.Application.Contracts.Slider;

namespace MasterManagement.Application.Contracts.SliderContracts
{
    public interface ISliderApplication
    {
        Task<OperationResult> CreateAsync(CreateSliderCommand command);
        Task<OperationResult> EditAsync(EditSliderCommand command);
        Task<OperationResult> ActivateAsync(long id);
        Task<OperationResult> DeactivateAsync(long id);
        Task<EditSliderCommand?> GetDetailsAsync(long id);
        Task<OperationResult> RemoveAsync(long id);
        Task<List<SliderViewModel>> GetSliderListAsync();
        Task<SliderViewModel?> GetByIdAsync(long id);
    }
}
