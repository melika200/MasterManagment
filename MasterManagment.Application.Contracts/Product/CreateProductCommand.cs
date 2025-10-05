namespace MasterManagment.Application.Contracts.Product;


public class CreateProductCommand
{
    public string? Name { get; set; }
    public string? ImagePath { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public long CategoryId { get; set; }
    public bool IsAvailable { get; set; }
}
