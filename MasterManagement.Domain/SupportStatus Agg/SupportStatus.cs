using _01_FrameWork.Domain;

namespace MasterManagement.Domain.SupportAgg;

public class SupportStatus : Enumeration
{

    public SupportStatus(int id, string name) : base(id, name) { }
  

    public ICollection<Support> Supports { get; private set; } = new List<Support>();

   

    protected SupportStatus() : base() { }

}
