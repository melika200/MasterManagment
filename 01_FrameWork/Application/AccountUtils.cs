using System.Security.Claims;


namespace _01_FrameWork.Application;

public class AccountUtils
{
    public static long GetAccountId(ClaimsPrincipal user)
    {
        long accountId = long.TryParse(user.FindFirst("AccountId")?.Value, out long amount) ? amount : 0L;
        return accountId;
    }

    public static string? HashPassword(string? password)
    {
        if (password == null) return null;
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string enteredPassword, string hashedPassword)
    {
        if (string.IsNullOrEmpty(enteredPassword))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }
        catch
        {
            return false;
        }
    }

}
