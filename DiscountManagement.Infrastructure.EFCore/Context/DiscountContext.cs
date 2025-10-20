using DiscountManagement.Domain.DiscountAgg;
using DiscountManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;
namespace DiscountManagement.Infrastructure.EFCore.Context;

public class DiscountContext : DbContext
{
    public DbSet<Discount> Discounts { get; set; }

    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscountMapping).Assembly);
    }
}
