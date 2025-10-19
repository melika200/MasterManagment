using System.Text.Json.Serialization;

namespace MasterManagment.Application.Contracts.Support;

public class EditUserSupportCommand
{
    [JsonIgnore]
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

