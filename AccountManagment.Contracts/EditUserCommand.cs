namespace AccountManagment.Contracts;

public class EditUserCommand:CreateUserCommand
{
    public long Id { get; set; }
}
