namespace MasterManagement.Application.Contracts.SliderContracts
{
    public class EditSliderViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
