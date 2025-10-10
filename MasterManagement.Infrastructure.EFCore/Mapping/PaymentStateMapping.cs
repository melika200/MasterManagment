using MasterManagement.Domain.PaymentStatusAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class PaymentStatusMapping : IEntityTypeConfiguration<PaymentStatus>
{
    public void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        builder.ToTable("PaymentStatuses");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasMany(ps => ps.Payments)
               .WithOne(p => p.Status)
               .HasForeignKey(p => p.PaymentStatusId);
    }
}
