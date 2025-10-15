using _01_FrameWork.Domain;

namespace MasterManagement.Domain.FaqUsAgg;

public class Faq : EntityBase
{
    public string Question { get; private set; }
    public string Answer { get; private set; }
    public bool IsActive { get; private set; }

    protected Faq() { }

    public Faq(string question, string answer)
    {
        Question = question;
        Answer = answer;
        IsActive = true;
    }

    public void Update(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
