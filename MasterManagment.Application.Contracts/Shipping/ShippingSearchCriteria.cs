namespace MasterManagment.Application.Contracts.Shipping
{
    public class ShippingSearchCriteria
    {
        public long? CartId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? ShippingStatusId { get; set; }
        //public DateTime? FromDate { get; set; }
        //public DateTime? ToDate { get; set; }
    }
}
