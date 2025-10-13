namespace MasterManagment.Application.Contracts.Slider;

public class SliderViewModel
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? ImagePath { get; set; }

    public string? Link { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? CreationDate { get; set; }
}
