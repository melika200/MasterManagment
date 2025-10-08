using MasterManagement.Domain.GalleryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterManagement.Infrastructure.EFCore.Mapping
{
    public class GalleryMapping : IEntityTypeConfiguration<Gallery>
    {
        public void Configure(EntityTypeBuilder<Gallery> builder)
        {
            builder.ToTable("Galleries");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.FilePath)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(p => p.Galleries)
                .HasForeignKey(x => x.ProductId)
                .IsRequired(false);
        }
    }
}
