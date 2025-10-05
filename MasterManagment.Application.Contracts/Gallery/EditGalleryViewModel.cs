namespace MasterManagment.Application.Contracts.Gallery;

public class EditGalleryViewModel
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
}
