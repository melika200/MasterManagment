using MasterManagement.Domain.OrderStateAgg;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class OrderStateMapping : IEntityTypeConfiguration<OrderState>
{
    public void Configure(EntityTypeBuilder<OrderState> builder)
    {
        builder.ToTable("OrderStates");

        builder.HasKey(os => os.Id);

        builder.Property(os => os.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasMany(os => os.Orders)
               .WithOne(o => o.OrderState)
               .HasForeignKey(o => o.OrderStateId);
    }
}