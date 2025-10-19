using System.Text.Json.Serialization;

namespace MasterManagment.Application.Contracts.Support;

public class EditAdminSupportCommand
{
    [JsonIgnore]
    public long Id { get; set; }
    public string? ReplyMessage { get; set; }
    public string? Status { get; set; } 
}
