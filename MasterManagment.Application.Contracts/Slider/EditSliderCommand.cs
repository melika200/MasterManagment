using System.Text.Json.Serialization;

namespace MasterManagment.Application.Contracts.Slider;

public class EditSliderCommand : CreateSliderCommand
{
    [JsonIgnore]
    public long Id { get; set; }
}
