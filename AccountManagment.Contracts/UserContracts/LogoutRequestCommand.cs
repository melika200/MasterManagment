namespace AccountManagment.Contracts.UserContracts;

public class LogoutRequestCommand
{
    public string RefreshToken { get; set; } = string.Empty;
}