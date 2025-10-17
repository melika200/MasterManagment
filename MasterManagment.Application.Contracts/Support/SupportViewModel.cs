namespace MasterManagment.Application.Contracts.Support;

public class SupportViewModel
{
    public long Id { get; set; }
    public long? AccountId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    /// <summary>
    /// موضوع بررسی چیه مشخص میکنه کاربر
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsReplied { get; set; }
    public DateTime CreationDate { get; set; }
}
