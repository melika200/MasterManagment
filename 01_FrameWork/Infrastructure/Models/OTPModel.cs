namespace _01_FrameWork.Infrastructure.Models;

public class OTPModel
{
    public string? Number { get; set; }
    public DateTime Date { get; set; }

    public bool IsExpired()
    {
        return DateTime.Now > Date.AddMinutes(5);
    }

    public bool IsValid(string? insertedOTP)
    {
        if (this.IsExpired()) return false;
        if (string.IsNullOrWhiteSpace(insertedOTP)) return false;
        if (this.Number != insertedOTP) return false;
        return true;
    }
}
