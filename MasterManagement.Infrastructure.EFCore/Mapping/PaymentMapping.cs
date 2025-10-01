using MasterManagement.Domain.PaymentAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class PaymentMapping : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.CartId).IsRequired();
        builder.Property(p => p.Amount).IsRequired();
        builder.Property(p => p.PaymentDate).IsRequired();
        builder.Property(p => p.TransactionId).HasMaxLength(500).IsRequired();
        builder.Property(p => p.IsSucceeded).IsRequired();
        builder.Property(p => p.IsCanceled).IsRequired();
    }
}



