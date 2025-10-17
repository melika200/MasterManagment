namespace MasterManagment.Application.Contracts.Support;

public class SupportSearchCriteria
{
    /// <summary>
    /// کاربر ب صورت دلحواه بگرده مثلا ی چیز بنویسه بعد میره تو ایمیل نام کاربری و چیزای دیگ بگرده میبینه هست یا ن 
    /// </summary>
    public string? Keyword { get; set; }
    //public bool? IsReplied { get; set; }
    public string? Status { get; set; }
    public long? AccountId { get; set; }

}
