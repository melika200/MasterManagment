using AccountManagment.Domain.UserAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.EFCore.Mapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
    
        builder.ToTable("Users");

        
        builder.HasKey(x => x.Id);

      
        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Fullname)
            .HasMaxLength(500);

        builder.Property(x => x.Password)
            .HasMaxLength(1000);

        builder.Property(x => x.RoleId)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        

        builder.HasOne(x => x.Role)      
            .WithMany(r => r.Users)           
            .HasForeignKey(x => x.RoleId);   
           

    }
}
