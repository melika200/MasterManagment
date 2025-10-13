using System.Text.Json.Serialization;

namespace MasterManagment.Application.Contracts.OrderContracts;

public class EditCartCommand : CreateCartCommand
{
    [JsonIgnore]
    public long Id { get; set; }
}
