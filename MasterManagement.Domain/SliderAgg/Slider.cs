using _01_FrameWork.Domain;

namespace MasterManagement.Domain.SliderAgg;

public class Slider : EntityBase,ISoftDelete
{
    public string Title { get; private set; }
    public bool IsDeleted { get; set; } = false;
    public string ImagePath { get; private set; }
    /// <summary>
    /// هدایت به صفحه دیگه مثلا بخواد جزییات اون تخفیف یا جشنواره مون بببینه
    /// </summary>
    public string Link { get; private set; }
    /// <summary>
    /// ترتیب نوشتن اسلایدرها
    /// </summary>
    public int DisplayOrder { get; private set; }
    /// <summary>
    /// ممکنه بخوای یه اسلایدر موقتاً نمایش داده نشه (بدون حذف کردنش).
    /// </summary>
    public bool IsActive { get; private set; }

    protected Slider() { }

    public Slider(string title, string imagePath, string link, int displayOrder)
    {
        Title = title;
        ImagePath = imagePath;
        Link = link;
        DisplayOrder = displayOrder;
        IsActive = true;
    }

    public void Edit(string title, string imagePath, string link, int displayOrder)
    {
        Title = title;
        if (!string.IsNullOrWhiteSpace(imagePath))
            ImagePath = imagePath;
        Link = link;
        DisplayOrder = displayOrder;
    }
    public void SoftDelete()
    {
        IsDeleted = true;
    }
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
