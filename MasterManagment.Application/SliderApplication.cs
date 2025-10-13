using _01_FrameWork.Application;
using MasterManagement.Application.Contracts.SliderContracts;
using MasterManagement.Domain.SliderAgg;
using MasterManagment.Application.Contracts.Slider;
using MasterManagment.Application.Contracts.UnitOfWork;
using Microsoft.AspNetCore.Hosting;

namespace MasterManagement.Application;

public class SliderApplication : ISliderApplication
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IMasterUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public SliderApplication(
        ISliderRepository sliderRepository,
        IMasterUnitOfWork unitOfWork,
        IWebHostEnvironment hostingEnvironment)
    {
        _sliderRepository = sliderRepository;
        _unitOfWork = unitOfWork;
        _hostingEnvironment = hostingEnvironment;
    }

   
    private string GetUploadPath()
    {
        var folder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "sliders");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }

    public async Task<OperationResult> CreateAsync(CreateSliderCommand command)
    {
        var operation = new OperationResult();

        if (command.File == null || command.File.Length == 0)
            return operation.Failed("فایل تصویر الزامی است.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(command.File.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
            return operation.Failed("فرمت فایل مجاز نیست.");

      
        var fileName = $"{Guid.NewGuid()}{ext}";
        var uploadPath = GetUploadPath();
        var filePath = Path.Combine(uploadPath, fileName);

       
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await command.File.CopyToAsync(stream);
        }

        var relativePath = Path.Combine("uploads", "sliders", fileName).Replace("\\", "/");

        var slider = new Slider(command.Title, relativePath, command.Link ?? string.Empty, command.DisplayOrder);
        await _sliderRepository.CreateAsync(slider);
        await _unitOfWork.CommitAsync();

        return operation.Succedded("اسلایدر با موفقیت اضافه شد.");
    }

    public async Task<OperationResult> EditAsync(EditSliderCommand command)
    {
        var operation = new OperationResult();
        var slider = await _sliderRepository.GetAsync(command.Id);
        if (slider == null)
            return operation.Failed("اسلایدر یافت نشد.");

        string? newImagePath = null;

        if (command.File != null && command.File.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(command.File.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return operation.Failed("فرمت فایل مجاز نیست.");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var uploadPath = GetUploadPath();
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await command.File.CopyToAsync(stream);
            }

            newImagePath = Path.Combine("uploads", "sliders", fileName).Replace("\\", "/");
        }

        slider.Edit(command.Title, newImagePath ?? slider.ImagePath, command.Link ?? string.Empty, command.DisplayOrder);
        await _unitOfWork.CommitAsync();

        return operation.Succedded("اسلایدر با موفقیت ویرایش شد.");
    }

    public async Task<OperationResult> ActivateAsync(long id)
    {
        var operation = new OperationResult();
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null)
            return operation.Failed("اسلایدر یافت نشد.");

        slider.Activate();
        await _sliderRepository.UpdateAsync(slider);
        await _unitOfWork.CommitAsync();

        return operation.Succedded("اسلایدر فعال شد.");
    }

    public async Task<OperationResult> DeactivateAsync(long id)
    {
        var operation = new OperationResult();
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null)
            return operation.Failed("اسلایدر یافت نشد.");

        slider.Deactivate();
        await _sliderRepository.UpdateAsync(slider);
        await _unitOfWork.CommitAsync();

        return operation.Succedded("اسلایدر غیرفعال شد.");
    }

    public async Task<OperationResult> RemoveAsync(long id)
    {
        var operation = new OperationResult();
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null)
            return operation.Failed("اسلایدر یافت نشد.");

        slider.SoftDelete();
        await _unitOfWork.CommitAsync();

        return operation.Succedded("اسلایدر حذف شد.");
    }

    public async Task<List<SliderViewModel>> GetSliderListAsync()
    {
        var sliders = await _sliderRepository.GetManyAsync(s => !s.IsDeleted);

        return sliders
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new SliderViewModel
            {
                Id = s.Id,
                Title = s.Title,
                ImagePath = s.ImagePath,
                Link = s.Link,
                DisplayOrder = s.DisplayOrder,
                IsActive = s.IsActive,
                CreationDate = s.CreationDate.ToString("yyyy/MM/dd")
            }).ToList();
    }

    public async Task<EditSliderCommand?> GetDetailsAsync(long id)
    {
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null)
            return null;

        return new EditSliderCommand
        {
            Id = slider.Id,
            Title = slider.Title,
            Link = slider.Link,
            DisplayOrder = slider.DisplayOrder
        };
    }


    public async Task<SliderViewModel?> GetByIdAsync(long id)
    {
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null) return null;

        return new SliderViewModel
        {
            Id = slider.Id,
            Title = slider.Title,
            ImagePath = slider.ImagePath,
            Link = slider.Link,
            DisplayOrder = slider.DisplayOrder,
            IsActive = slider.IsActive
        };
    }

}





