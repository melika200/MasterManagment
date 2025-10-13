using MasterManagement.Domain.CartAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class CartMapping : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.AccountId).IsRequired();
        builder.Property(c => c.TotalAmount).IsRequired();
        builder.Property(c => c.DiscountAmount).IsRequired();
        builder.Property(c => c.PayAmount).IsRequired();
        builder.Property(c => c.IsPaid).IsRequired();
        builder.Property(c => c.IsCanceled).IsRequired();
        builder.Property(c => c.IssueTrackingNo).HasMaxLength(500);
        builder.Property(c => c.RefId).IsRequired();
        builder.Property(c => c.PaymentMethodId).IsRequired();

        //builder.HasOne(c => c.PaymentMethod)
        //       .WithMany(m => m.Carts)
        //       .HasForeignKey(c => c.PaymentMethodId);
        builder.Property(c => c.PaymentMethodId).IsRequired();
        builder.Property(c => c.PaymentMethodName)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasMany(c => c.Items)
               .WithOne(i => i.Cart)
               .HasForeignKey(i => i.CartId);
               
    }
}

