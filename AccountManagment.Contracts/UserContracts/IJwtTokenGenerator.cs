using AccountManagment.Domain.UserAgg;

namespace AccountManagment.Contracts.UserContracts;

public interface IJwtTokenGenerator
{
    Task<TokenResultViewModel> GenerateTokensAsync(User user);
}

