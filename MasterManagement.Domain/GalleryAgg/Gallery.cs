using _01_FrameWork.Domain;
using MasterManagement.Domain.ProductAgg;

namespace MasterManagement.Domain.GalleryAgg;


public class Gallery : EntityBase
{
    public long ProductId { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public Product Product { get; private set; }
    //public DateTime UploadedOn { get; private set; }



    public Gallery(long productId, string fileName, string filePath)
    {
        if (productId <= 0)
            throw new ArgumentException("شناسه محصول معتبر نیست.", nameof(productId));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("نام فایل نمی‌تواند خالی باشد.", nameof(fileName));

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("مسیر فایل نمی‌تواند خالی باشد.", nameof(filePath));

        ProductId = productId;
        FileName = fileName;
        FilePath = filePath;
        //UploadedOn = DateTime.UtcNow;
    }


    public void UpdateFile(string newFileName, string newFilePath)
    {
        if (string.IsNullOrWhiteSpace(newFileName))
            throw new ArgumentException("نام فایل جدید نمی‌تواند خالی باشد.", nameof(newFileName));

        if (string.IsNullOrWhiteSpace(newFilePath))
            throw new ArgumentException("مسیر فایل جدید نمی‌تواند خالی باشد.", nameof(newFilePath));

        FileName = newFileName;
        FilePath = newFilePath;
        //UploadedOn = DateTime.UtcNow;
    }
}



