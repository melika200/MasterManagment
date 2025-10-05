using System.Text.Json.Serialization;

namespace MasterManagment.Application.Contracts.Product;


public class EditProductCommand : CreateProductCommand
{

    [JsonIgnore]
    public long Id { get; set; }
  
}
