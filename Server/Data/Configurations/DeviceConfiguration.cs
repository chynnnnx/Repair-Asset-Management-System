using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projServer.Entities;

namespace projServer.Data.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<DeviceEntity>
    {
        public void Configure(EntityTypeBuilder<DeviceEntity> builder)
        {
          
            builder.ToTable("PCs");

            builder.HasKey(pc => pc.DeviceID);

            builder.Property(pc => pc.Tag)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(pc => pc.Status)
                .IsRequired()
                .HasConversion<string>(); 

            builder.HasOne(pc => pc.Room)
                   .WithMany(r => r.PCs)   
                   .HasForeignKey(pc => pc.RoomId)
                   .OnDelete(DeleteBehavior.Cascade); 
          
            builder.Property(pc => pc.DeviceID)
                   .ValueGeneratedOnAdd();
        }
    }
}
