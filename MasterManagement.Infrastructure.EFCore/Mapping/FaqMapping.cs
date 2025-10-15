using MasterManagement.Domain.FaqUsAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class FaqMapping : IEntityTypeConfiguration<Faq>
{
    public void Configure(EntityTypeBuilder<Faq> builder)
    {
        builder.ToTable("Faqs");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Question)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(f => f.Answer)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(f => f.CreationDate)
            .IsRequired();
    }
}
