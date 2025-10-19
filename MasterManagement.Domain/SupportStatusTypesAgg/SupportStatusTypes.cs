using MasterManagement.Domain.SupportAgg;

namespace MasterManagement.Domain.SupportStatusTypesAgg;


public static class SupportStatusType
{
    public static readonly SupportStatus Open = new SupportStatus(1, "Opened");
    public static readonly SupportStatus InProgress = new SupportStatus(2, "Progress");
    public static readonly SupportStatus Closed = new SupportStatus(3, "Answer And Closed");

    public static readonly List<SupportStatus> AllTypes = new()
    {
        Open,
        InProgress,
        Closed
    };
}



