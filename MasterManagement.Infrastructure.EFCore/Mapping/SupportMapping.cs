using MasterManagement.Domain.SupportAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping;

public class SupportMapping : IEntityTypeConfiguration<Support>
{
    public void Configure(EntityTypeBuilder<Support> builder)
    {
        builder.ToTable("Supports");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Subject).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Message).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.IsReplied).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();

        builder.HasOne(x => x.Status)
               .WithMany(x => x.Supports)
               .HasForeignKey(x => x.SupportStatusId);
            
    }
}
