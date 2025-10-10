using MasterManagement.Domain.OrderAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.AccountId).IsRequired();
        builder.Property(o => o.TotalAmount).IsRequired();
        builder.Property(o => o.DiscountAmount).IsRequired();
        builder.Property(o => o.PayAmount).IsRequired();
        builder.Property(o => o.IsPaid).IsRequired();
        builder.Property(o => o.IsCanceled).IsRequired();
        builder.Property(o => o.IssueTrackingNo).HasMaxLength(500);
        builder.Property(o => o.RefId).IsRequired();
        builder.Property(o => o.ShippingStatusId).IsRequired();
        builder.Property(x => x.OrderStateId).IsRequired();

        builder.Property(o => o.PaymentMethodId).IsRequired();

        builder.HasOne(o => o.PaymentMethod)
               .WithMany(m => m.Orders)
               .HasForeignKey(o => o.PaymentMethodId);


        builder.HasOne(x => x.OrderState)
               .WithMany(os => os.Orders)
               .HasForeignKey(x => x.OrderStateId);

        builder.HasOne(x => x.ShippingStatus)
            .WithMany(os => os.Orders)
            .HasForeignKey(x => x.ShippingStatusId);


        builder.HasMany(o => o.Items)
               .WithOne(i => i.Order)
               .HasForeignKey(i => i.OrderId);
               
    }
}


