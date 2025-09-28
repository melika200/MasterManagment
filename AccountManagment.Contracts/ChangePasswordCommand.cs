namespace AccountManagment.Contracts;

public class ChangePasswordCommand
{
    public long Id { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirm { get; set; }
}
