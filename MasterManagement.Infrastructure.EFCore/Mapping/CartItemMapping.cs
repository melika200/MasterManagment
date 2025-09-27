using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class CartItemMapping : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.ProductId).IsRequired();
        builder.Property(ci => ci.ProductName).HasMaxLength(255).IsRequired();
        builder.Property(ci => ci.Count).IsRequired();
        builder.Property(ci => ci.UnitPrice).IsRequired();
        builder.Property(ci => ci.DiscountRate).IsRequired();

        builder.HasOne(ci => ci.Cart)
               .WithMany(c => c.Items)
               .HasForeignKey(ci => ci.CartId);

        //builder.HasOne(ci => ci.Product)
        //       .WithMany()
        //       .HasForeignKey(ci => ci.ProductId)
        //       .IsRequired(false);


    }
}
