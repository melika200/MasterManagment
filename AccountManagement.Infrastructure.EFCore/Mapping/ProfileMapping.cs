using AccountManagment.Domain.ProfileAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagment.Infrastructure.EFCore.Mapping
{
    public class ProfileMapping : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profiles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName).HasMaxLength(200);
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.Address).HasMaxLength(500);
            builder.Property(x => x.PostalCode).HasMaxLength(50);
            builder.Property(x => x.AvatarPath).HasMaxLength(300);

            builder.HasOne(x => x.User)
                   .WithOne(u => u.Profile)
                   .HasForeignKey<Profile>(x => x.UserId);
                  

        }
    }
}
