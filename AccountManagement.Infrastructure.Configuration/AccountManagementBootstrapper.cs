using AccountManagement.Application;
using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagement.Infrastructure.EFCore.Repository;
using AccountManagment.Contracts;
using AccountManagment.Domain.UserAgg;
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


        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        services.AddDbContext<AccountContext>(options => options.UseSqlServer(connectionString));
    }
}
