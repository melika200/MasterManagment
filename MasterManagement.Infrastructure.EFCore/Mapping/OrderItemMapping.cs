using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasterManagement.Domain.OrderAgg;

namespace MasterManagement.Infrastructure.EFCore.Mapping
{
    public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.ProductId).IsRequired();
            builder.Property(oi => oi.ProductName).HasMaxLength(255).IsRequired();
            builder.Property(oi => oi.Count).IsRequired();
            builder.Property(oi => oi.UnitPrice).IsRequired();
            builder.Property(oi => oi.DiscountRate).IsRequired();

            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.Items)
                   .HasForeignKey(oi => oi.OrderId);
        }
    }
}
