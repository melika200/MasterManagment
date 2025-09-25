using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasterManagement.Domain.OrderAgg;

namespace MasterManagement.Infrastructure.EFCore.Mapping
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.AccountId).IsRequired();
            builder.Property(o => o.PaymentMethod).IsRequired();
            builder.Property(o => o.TotalAmount).IsRequired();
            builder.Property(o => o.DiscountAmount).IsRequired();
            builder.Property(o => o.PayAmount).IsRequired();
            builder.Property(o => o.IsPaid).IsRequired();
            builder.Property(o => o.IsCanceled).IsRequired();
            builder.Property(o => o.IssueTrackingNo).HasMaxLength(500);
            builder.Property(o => o.RefId).IsRequired();

            builder.HasMany(o => o.Items)
                   .WithOne(i => i.Order)
                   .HasForeignKey(i => i.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

