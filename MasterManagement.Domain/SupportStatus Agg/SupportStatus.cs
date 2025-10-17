using _01_FrameWork.Domain;

namespace MasterManagement.Domain.SupportAgg;

public class SupportStatus : ISoftDelete
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public bool IsDeleted { get; set; }

    public ICollection<Support> Supports { get; private set; } = new List<Support>();

    public SupportStatus(int id, string name)
    {
        Id = id;
        Name = name;
    }

    protected SupportStatus() { }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
