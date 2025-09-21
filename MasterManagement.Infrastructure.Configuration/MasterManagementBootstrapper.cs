using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using MasterManagement.Infrastructure.EFCore.Repository;
using MasterManagment.Application;
using MasterManagment.Application.Contracts.ProductCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MasterManagement.Infrastructure.Configuration
{
    public class MasterManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();

           
            services.AddDbContext<MasterContext>(x => x.UseSqlServer(connectionString));
        }
    }
 }  
