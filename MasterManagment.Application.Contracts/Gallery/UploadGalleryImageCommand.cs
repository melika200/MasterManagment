using Microsoft.AspNetCore.Http;

namespace MasterManagment.Application.Contracts.Gallery;

public class UploadGalleryImageCommand
{
    public long ProductId { get; set; }
    public IFormFile File { get; set; } = null!;
}
