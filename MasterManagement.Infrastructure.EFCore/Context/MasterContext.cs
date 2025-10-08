using System.Linq.Expressions;
using _01_FrameWork.Domain;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.GalleryAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.OrderItemAgg;
using MasterManagement.Domain.OrderStateAgg;
using MasterManagement.Domain.OrderStatesTypeAgg;
using MasterManagement.Domain.PaymentAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagement.Domain.ShippingStatusAgg;
using MasterManagement.Domain.ShippingStatusesTypeAgg;
using MasterManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Context;

public class MasterContext : DbContext
{

    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<OrderState> OrderState { get; set; }
    public DbSet<ShippingStatus> ShippingStatus { get; set; }
    public MasterContext(DbContextOptions<MasterContext> options) : base(options)
    {
    }
    private static LambdaExpression GetIsDeletedFilter(Type type)
    {
        var parameter = Expression.Parameter(type, "e");
        var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
        var condition = Expression.Equal(property, Expression.Constant(false));
        return Expression.Lambda(condition, parameter);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        var softDeleteEntities = modelBuilder.Model.GetEntityTypes()
        .Where(e => typeof(ISoftDelete).IsAssignableFrom(e.ClrType));

        foreach (var entityType in softDeleteEntities)
        {
            var entityBuilder = modelBuilder.Entity(entityType.ClrType);
            entityBuilder.HasQueryFilter(GetIsDeletedFilter(entityType.ClrType));
        }


        var assembly = typeof(ProductCategoryMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        modelBuilder.Entity<OrderState>().HasData(OrderStatesType.AllStates);
        modelBuilder.Entity<ShippingStatus>().HasData(ShippingStatusesType.AllStatuses);
        base.OnModelCreating(modelBuilder);
    }
}
