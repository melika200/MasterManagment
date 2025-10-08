using _01_FrameWork.Domain;
using MasterManagement.Domain.OrderAgg;

namespace MasterManagement.Domain.OrderStateAgg;

    public class OrderState : Enumeration
    {
        public OrderState(int id, string name) : base(id, name) { }

        public ICollection<Order> Orders { get; private set; } = new List<Order>();

        private OrderState() : base() { }
    }



//public class OrderState : Enumeration
//{
//    public OrderState(int id, string name) : base(id, name) { }
//    public ICollection<Order> Orders { get; private set; } = new List<Order>();
//}


//public abstract class Enumeration : IComparable
//{
//    public int Id { get; private set; }
//    public string Name { get; private set; }

//    protected Enumeration(int id, string name)
//    {
//        Id = id;
//        Name = name;
//    }

//    public override string ToString() => Name;

//    public int CompareTo(object? other) => Id.CompareTo(((Enumeration)other!).Id);
//}
