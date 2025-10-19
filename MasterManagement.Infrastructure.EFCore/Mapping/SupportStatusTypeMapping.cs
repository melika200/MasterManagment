using MasterManagement.Domain.SupportAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping
{
    public class SupportStatusMapping : IEntityTypeConfiguration<SupportStatus>
    {
        public void Configure(EntityTypeBuilder<SupportStatus> builder)
        {
            builder.ToTable("SupportStatusTypes");

            builder.HasKey(x => x.Id);

     
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            //builder.Property(x => x.IsDeleted).IsRequired();

            builder.HasMany(x => x.Supports)
                   .WithOne(x => x.Status)
                   .HasForeignKey(x => x.SupportStatusId);
        }
    }
}
