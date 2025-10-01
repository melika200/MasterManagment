using _01_FrameWork.Domain;
using MasterManagement.Domain.ProductAgg;


namespace MasterManagement.Domain.ProductCategoryAgg;

public class ProductCategory:EntityBase,ISoftDelete
{
    public string Name { get; private set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public string Description { get; private set; } = string.Empty;
    public string Picture { get; private set; } = string.Empty;
    public string PictureAlt { get; private set; } =string.Empty;
    public string PictureTitle { get; private set; } =string.Empty;
    public string Keywords { get; private set; } =string.Empty;
    public string MetaDescription { get; private set; } =string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public List<Product> Products { get; private set; }
    public void SoftDelete()
    {
        IsDeleted = true;
    }

    public ProductCategory()
    {
        Products = new List<Product>();
    }

    public ProductCategory(string name, string description, string picture,
          string pictureAlt, string pictureTitle, string keywords, string metaDescription,
          string slug)
    {
        Name = name;
        Description = description;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Keywords = keywords;
        MetaDescription = metaDescription;
        Slug = slug;
        Products = new List<Product>();
    }


    public void Edit(string name, string description, string picture,
     string pictureAlt, string pictureTitle, string keywords, string metaDescription,
     string slug)
    {
        Name = name;
        Description = description;

        if (!string.IsNullOrWhiteSpace(picture))
            Picture = picture;

        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Keywords = keywords;
        MetaDescription = metaDescription;
        Slug = slug;
       
    }
}
