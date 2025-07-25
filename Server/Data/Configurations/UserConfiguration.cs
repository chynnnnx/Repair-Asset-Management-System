using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projServer.Entities;

namespace projServer.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(s => s.UserID);


            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.MiddleName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.Gender)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.Role)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.ProfilePicture)
                 .IsRequired()
                 .HasColumnType("varbinary(max)");

        }
    }
}
