namespace MasterManagment.Application.Contracts.ProductCategory;

public class ProductCategoryViewModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Picture { get; set; }
    public string? CreationDate { get; set; }
    public string? ImagePath { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public long CategoryId { get; set; }
    public string? CategoryName { get; set; }
    //public bool IsAvailable { get; set; }

    //public long ProductsCount { get; set; }
}
