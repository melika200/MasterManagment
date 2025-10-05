using System.Text.Json.Serialization;

namespace AccountManagment.Contracts.UserContracts;

public class EditUserCommand:CreateUserCommand
{
    [JsonIgnore]
    public long Id { get; set; }
    //public long RoleId { get; set; }
    
}
