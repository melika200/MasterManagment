using AccountManagment.Domain.UserAgg;

namespace AccountManagment.Contracts;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);

}
