using System.Text.Json.Serialization;

namespace MasterManagment.Application.Contracts.Support
{
    class EditSupportCommand:CreateSupportCommand
    {
        [JsonIgnore]
        public long Id { get; set; }
    }
}
