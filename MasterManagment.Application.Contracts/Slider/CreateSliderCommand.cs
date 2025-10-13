using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MasterManagment.Application.Contracts.Slider;

public class CreateSliderCommand
{
    [Required(ErrorMessage = "عنوان اسلایدر الزامی است.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "فایل تصویر الزامی است.")]
    public IFormFile? File { get; set; }

    public string? Link { get; set; }

    public int DisplayOrder { get; set; }
}
