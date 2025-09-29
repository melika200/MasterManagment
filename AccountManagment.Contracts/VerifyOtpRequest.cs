namespace AccountManagment.Contracts;


public class VerifyOtpRequest
{
    public string? Mobile { get; set; }
    public string? OtpCode { get; set; }
}
