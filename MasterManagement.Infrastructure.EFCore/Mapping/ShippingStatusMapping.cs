using MasterManagement.Domain.ShippingStatusAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class ShippingStatusMapping : IEntityTypeConfiguration<ShippingStatus>
{
    public void Configure(EntityTypeBuilder<ShippingStatus> builder)
    {
        builder.ToTable("ShippingStatuses");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasMany(s => s.Orders)
               .WithOne(o => o.ShippingStatus)
               .HasForeignKey(o => o.ShippingStatusId);
    }
}


