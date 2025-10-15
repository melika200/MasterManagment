using MasterManagement.Domain.AboutUsAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class AboutMapping : IEntityTypeConfiguration<About>
{
    public void Configure(EntityTypeBuilder<About> builder)
    {
        builder.ToTable("Abouts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(a => a.CreationDate)
            .IsRequired();
    }
}
