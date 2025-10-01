namespace AccountManagment.Contracts.UserContracts;

public class EditUserCommand:CreateUserCommand
{
    public long Id { get; set; }
}
