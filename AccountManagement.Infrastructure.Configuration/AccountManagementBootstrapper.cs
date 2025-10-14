using AccountManagement.Application;
using AccountManagement.Infrastructure.EFCore;
using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagement.Infrastructure.EFCore.Repository;
using AccountManagment.Application;
using AccountManagment.Application.Contracts.Profile;
using AccountManagment.Contracts.UnitOfWork;
using AccountManagment.Contracts.UserContracts;
using AccountManagment.Domain.UserAgg;
using AccountManagment.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Infrastructure.Configuration;

public class AccountManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddTransient<IUserApplication, UserApplication>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddTransient<IAccountUnitOfWork, AccountUnitOfWork>();
        services.AddTransient<IProfileApplication, ProfileApplication>();
        services.AddTransient<IProfileRepository, ProfileRepository>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        services.AddDbContext<AccountContext>(options => options.UseSqlServer(connectionString));
    }
}
