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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccountId).IsRequired();
            builder.Property(x => x.PaymentMethod).IsRequired();
            builder.Property(x => x.TotalAmount).IsRequired();
            builder.Property(x => x.DiscountAmount).IsRequired();
            builder.Property(x => x.PayAmount).IsRequired();
            builder.Property(x => x.IsPaid).IsRequired();
            builder.Property(x => x.IsCanceled).IsRequired();
            builder.Property(x => x.IssueTrackingNo).HasMaxLength(1000);
            builder.Property(x => x.RefId);

        
            builder
                .HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
