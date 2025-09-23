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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.ProductName).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Count).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired();
            builder.Property(x => x.DiscountRate).IsRequired();

         
            builder.HasOne(x => x.Order)
                   .WithMany(o => o.Items)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
