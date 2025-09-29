using AccountManagment.Domain.UserAgg;

namespace AccountManagment.Contracts;

public interface IJwtTokenGenerator
{
    Task<TokenResult> GenerateTokensAsync(User user);
}

