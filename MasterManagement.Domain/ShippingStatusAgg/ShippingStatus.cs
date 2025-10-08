namespace MasterManagement.Domain.ShippingStatusAgg;
using _01_FrameWork.Domain;
using MasterManagement.Domain.OrderAgg;


public class ShippingStatus : Enumeration
    {
        public ShippingStatus(int id, string name) : base(id, name) { }

        public ICollection<Order> Orders { get; private set; } = new List<Order>();

       
        private ShippingStatus() : base() { }
    }



