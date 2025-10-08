using MasterManagement.Domain.CartAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class ShippingMapping : IEntityTypeConfiguration<Shipping>
{
    public void Configure(EntityTypeBuilder<Shipping> builder)
    {
        builder.ToTable("Shippings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.CartId).IsRequired();
        builder.Property(s => s.FullName).IsRequired().HasMaxLength(255);
        builder.Property(s => s.PhoneNumber).IsRequired().HasMaxLength(50);
        builder.Property(s => s.Address).IsRequired().HasMaxLength(500);
        builder.Property(s => s.Description).HasMaxLength(1000);
        builder.Property(s => s.ShippingStatusId).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();


        builder.HasOne(s => s.ShippingStatus)
              .WithMany(ss => ss.Shippings)
              .HasForeignKey(s => s.ShippingStatusId);

        builder.HasOne(s => s.Cart)
          .WithOne()
          .HasForeignKey<Shipping>(s => s.CartId)
          .IsRequired(false);

    }
}
