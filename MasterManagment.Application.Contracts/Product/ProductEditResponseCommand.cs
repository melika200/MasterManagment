using MasterManagment.Application.Contracts.Product;

namespace MasterManagment.Application.Contracts.Product
{
    public class ProductEditResponseCommand
    {
        public bool IsSuccedded { get; set; }
        public string? Message { get; set; }
        public ProductViewModel? Product { get; set; }
    }

}
