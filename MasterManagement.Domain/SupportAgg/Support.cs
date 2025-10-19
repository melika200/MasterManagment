using _01_FrameWork.Domain;
using MasterManagement.Domain.SupportStatusTypesAgg;

namespace MasterManagement.Domain.SupportAgg;

public class Support : EntityBase, ISoftDelete
{
    public long AccountId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public string? ReplyMessage { get; private set; }
    public int SupportStatusId { get; private set; }
    public SupportStatus? Status { get; private set; }
    public bool IsReplied { get; private set; }
    public bool IsDeleted { get; set; }

    public Support(long accountId, string fullName, string email, string phoneNumber, string subject, string message)
    {
        AccountId = accountId;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Subject = subject;
        Message = message;
        IsReplied = false;
        IsDeleted = false;
        SupportStatusId = SupportStatusType.Open.Id;
        //Status = SupportStatusType.Open;
    }

    public void Edit(string fullName, string email, string phoneNumber, string subject, string message)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Subject = subject;
        Message = message;
    }

    public void MarkAsReplied() => IsReplied = true;

    public void Reply(string message)
    {
        ReplyMessage = message;
        IsReplied = true;
    }

    public void ChangeStatus(SupportStatus newStatus)
    {
        //Status = newStatus;
        SupportStatusId = newStatus.Id;
    }

    public void SoftDelete() => IsDeleted = true;
}
