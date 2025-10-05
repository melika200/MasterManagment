using _01_FrameWork.Application;
using MasterManagement.Domain.GalleryAgg;
using MasterManagment.Application.Contracts.Gallery;
using MasterManagment.Application.Contracts.UnitOfWork;
using Microsoft.AspNetCore.Hosting;

namespace MasterManagment.Application
{
    public class GalleryApplication : IGalleryApplication
    {
        private readonly IMasterUnitOfWork _unitOfWork;
        private readonly IGalleryRepository _GalleryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public GalleryApplication(IMasterUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment, IGalleryRepository galleryRepository )
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _GalleryRepository = galleryRepository;
        }

        public async Task<OperationResult> UploadImageAsync(UploadGalleryImageCommand command)
        {
            var operation = new OperationResult();

            if (command.File == null || command.File.Length == 0)
                return operation.Failed("فایل ارسال نشده است.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(command.File.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return operation.Failed("فرمت فایل مجاز نیست.");

            var newFileName = $"{Guid.NewGuid()}{ext}";

            
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, newFileName);

            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await command.File.CopyToAsync(stream);
            }

            
            var gallery = new Gallery(command.ProductId, newFileName, filePath);

         
            await _GalleryRepository.CreateAsync(gallery);
            await _unitOfWork.CommitAsync();

            return operation.Succedded("تصویر با موفقیت آپلود شد.");
        }

        //public async Task<OperationResult> UploadImageAsync(UploadGalleryImageCommand command)
        //{
        //    var operation = new OperationResult();

        //    if (command.File == null || command.File.Length == 0)
        //        return operation.Failed("فایل ارسال نشده است.");

        //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif",".webp" };
        //    var ext = Path.GetExtension(command.File.FileName).ToLowerInvariant();
        //    if (!allowedExtensions.Contains(ext))
        //        return operation.Failed("فرمت فایل مجاز نیست.");


        //    var newFileName = $"{Guid.NewGuid()}{ext}";


        //    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        //    if (!Directory.Exists(uploadsFolder))
        //        Directory.CreateDirectory(uploadsFolder);

        //    var filePath = Path.Combine(uploadsFolder, newFileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await command.File.CopyToAsync(stream);
        //    }


        //    var gallery = new Gallery(command.ProductId, newFileName, filePath);
        //    await _GalleryRepository.CreateAsync(gallery);
        //    await _unitOfWork.CommitAsync();

        //    return operation.Succedded("تصویر با موفقیت آپلود شد.");
        //}
    }

}
