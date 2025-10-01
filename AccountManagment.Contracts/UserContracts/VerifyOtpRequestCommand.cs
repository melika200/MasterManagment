namespace AccountManagment.Contracts.UserContracts;


public class VerifyOtpRequestCommand
{
    public string? Mobile { get; set; }
    public string? OtpCode { get; set; }
}
