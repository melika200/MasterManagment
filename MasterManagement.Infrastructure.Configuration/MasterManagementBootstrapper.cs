using MasterManagement.Application.Contracts.SliderContracts;
using MasterManagement.Application;
using MasterManagement.Domain.GalleryAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.PaymentAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagement.Domain.ShippingAgg;
using MasterManagement.Domain.SliderAgg;
using MasterManagement.Infrastructure.EFCore;
using MasterManagement.Infrastructure.EFCore.Context;
using MasterManagement.Infrastructure.EFCore.Repository;
using MasterManagment.Application;
using MasterManagment.Application.Contracts.Gallery;
using MasterManagment.Application.Contracts.OrderContracts;
using MasterManagment.Application.Contracts.Payment;
using MasterManagment.Application.Contracts.Product;
using MasterManagment.Application.Contracts.ProductCategory;
using MasterManagment.Application.Contracts.Shipping;
using MasterManagment.Application.Contracts.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace MasterManagement.Infrastructure.Configuration;

public class MasterManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
        services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddTransient<IProductApplication, ProductApplication>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<ICartApplication, CartApplication>();
        services.AddTransient<ICartRepository,CartRepository>();
        services.AddTransient<IOrderApplication,OrderApplication>();
        services.AddTransient<IOrderRepository,OrderRepository>();
        services.AddTransient<IPaymentApplication,PaymentApplication>();
        services.AddTransient<IPaymentRepository,PaymentRepository>();
        services.AddTransient<IGalleryRepository, GalleryRepository>();
        services.AddTransient<IGalleryApplication, GalleryApplication>();
        services.AddTransient<IShippingApplication, ShippingApplication>();
        services.AddTransient<IShippingRepository, ShippingRepository>();
        services.AddTransient<ISliderRepository, SliderRepository>();
        services.AddTransient<ISliderApplication, SliderApplication>();



        services.AddTransient<IMasterUnitOfWork, MasterUnitOfWork>();


        services.AddDbContext<MasterContext>(x => x.UseSqlServer(connectionString));
    }
}

