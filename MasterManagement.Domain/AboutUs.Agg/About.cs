using _01_FrameWork.Domain;

namespace MasterManagement.Domain.AboutUsAgg;

public class About : EntityBase
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }

    protected About() { }

    public About(string title, string description)
    {
        Title = title;
        Description = description;
        IsActive = true;
    }

    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
