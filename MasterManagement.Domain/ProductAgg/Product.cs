using _01_FrameWork.Domain;
using MasterManagement.Domain.GalleryAgg;
using MasterManagement.Domain.ProductCategoryAgg;

namespace MasterManagement.Domain.ProductAgg;


public class Product : EntityBase,ISoftDelete
{
    public string Name { get; private set; }
    public bool IsDeleted { get; set; } = false;
    public string ImagePath { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public int Stock { get; private set; }
    public long CategoryId { get; private set; }
    public ProductCategory? Category { get; private set; }
    public bool IsAvailable { get; private set; }
    public int TotalRatings { get; private set; }
    public double AverageRating { get; private set; }
    public ICollection<Gallery>? Galleries { get; private set; }


    public Product(
        string name,
        string imagePath,
        decimal price,
        string description,
        int stock,
        long categoryId,
        bool isAvailable)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("نام محصول نمی‌تواند خالی باشد.", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "قیمت نمی‌تواند منفی باشد.");
        if (stock < 0)
            throw new ArgumentOutOfRangeException(nameof(stock), "موجودی نمی‌تواند منفی باشد.");

        Name = name;
        ImagePath = imagePath;
        Price = price;
        Description = description;
        Stock = stock;
        CategoryId = categoryId;
        IsAvailable = isAvailable;
        TotalRatings = 0;
        AverageRating = 0.0;
        Galleries = new List<Gallery>();

    }

    public void Edit(
        string name,
        string imagePath,
        decimal price,
        string description,
        int stock,
        long categoryId,
        bool isAvailable)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("نام محصول نمی‌تواند خالی باشد.", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "قیمت نمی‌تواند منفی باشد.");
        if (stock < 0)
            throw new ArgumentOutOfRangeException(nameof(stock), "موجودی نمی‌تواند منفی باشد.");

        Name = name;
        if (!string.IsNullOrWhiteSpace(imagePath))
            ImagePath = imagePath;

        Price = price;
        Description = description;
        Stock = stock;
        CategoryId = categoryId;
        IsAvailable = isAvailable;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
    public void UpdateRatings(int totalRatings, double averageRating)
    {
        if (totalRatings < 0)
            throw new ArgumentOutOfRangeException(nameof(totalRatings), "تعداد امتیازات نمی‌تواند منفی باشد.");
        if (averageRating < 0 || averageRating > 5)
            throw new ArgumentOutOfRangeException(nameof(averageRating), "امتیاز میانگین باید بین 0 تا 5 باشد.");

        TotalRatings = totalRatings;
        AverageRating = averageRating;
    }
}













