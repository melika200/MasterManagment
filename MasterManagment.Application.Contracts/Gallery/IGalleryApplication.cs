using _01_FrameWork.Application;

namespace MasterManagment.Application.Contracts.Gallery;

public interface IGalleryApplication
{
    Task<OperationResult> UploadImageAsync(UploadGalleryImageCommand command);
    //Task<OperationResult> EditAsync(EditGalleryCommand command);
    //Task<GalleryViewModel?> GetByIdAsync(long id);
    //Task<List<GalleryViewModel>> SearchAsync(GallerySearchCriteria criteria);
    //Task<OperationResult> DeleteAsync(long id);
}
