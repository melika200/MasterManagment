using MasterManagement.Domain.SupportAgg;

namespace MasterManagement.Domain.SupportStatusTypesAgg;


public static class SupportStatusType
{
    public static readonly SupportStatus Open = new SupportStatus(1, "Open");
    public static readonly SupportStatus InProgress = new SupportStatus(2, "InProgress");
    public static readonly SupportStatus Closed = new SupportStatus(3, "Closed");

    public static readonly List<SupportStatus> AllTypes = new()
    {
        Open,
        InProgress,
        Closed
    };
}



