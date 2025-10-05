namespace MasterManagment.Application.Contracts.Gallery;

public class GalleryViewModel
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    //public DateTime UploadedOn { get; set; }
}
