using System.Linq.Expressions;
using _01_FrameWork.Domain;
using AccountManagement.Infrastructure.EFCore.Mapping;
using AccountManagment.Domain.RoleAgg;
using AccountManagment.Domain.RolesTypesAgg;
using AccountManagment.Domain.UserAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Context;

public class AccountContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public AccountContext(DbContextOptions<AccountContext> options) : base(options)
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
        var assembly = typeof(UserMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        modelBuilder.Entity<Role>().HasData(RolesType.AllTypes);
        base.OnModelCreating(modelBuilder);
    }
}
