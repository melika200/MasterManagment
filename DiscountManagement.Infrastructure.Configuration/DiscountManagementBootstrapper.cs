using DiscountManagement.Application;
using DiscountManagement.Application.Contracts.Discount;
using DiscountManagement.ApplicationContracts.DiscountUnitOfWork;
using DiscountManagement.Domain.DiscountAgg;
using DiscountManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore.Context;
using DiscountManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DiscountManagement.Infrastructure.Configuration;

public static class DiscountManagmentBootstrapper
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DiscountContext>(x => x.UseSqlServer(connectionString));


        //services.AddAutoMapper(typeof(DiscountMappingProfile));
        services.AddAutoMapper(cfg => { }, typeof(DiscountMappingProfile));

        services.AddTransient<IDiscountApplication, DiscountApplication>();
        services.AddTransient<IDiscountRepository, DiscountRepository>();
        services.AddTransient<IDiscountUnitOfWork, DiscountUnitOfWork>();


    }
}
