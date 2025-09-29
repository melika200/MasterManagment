using Microsoft.Extensions.DependencyInjection;

namespace _01_FrameWork.Infrastructure.Configuration;

public class FrameworkBootstrapper
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddTransient<ISMSService, SMSService>();
    }

}
