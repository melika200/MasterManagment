using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Context
{
    public class MasterContext : DbContext
    {

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


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
            base.OnModelCreating(modelBuilder);
        }
    }
}
