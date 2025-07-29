using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projServer.Entities;

namespace projServer.Data.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<DeviceEntity>
    {
        public void Configure(EntityTypeBuilder<DeviceEntity> builder)
        {
          
            builder.ToTable("Devices");

            builder.HasKey(devices => devices.DeviceID);

            builder.Property(devices => devices.Tag)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(devices => devices.Status)
                .IsRequired()
                .HasConversion<string>(); 
            builder.HasOne(devices => devices.Room)
                   .WithMany(r => r.Devices)   
                   .HasForeignKey(devices => devices.RoomId)
                   .OnDelete(DeleteBehavior.Cascade); 
            builder.Property(devices => devices.DeviceID)
                   .ValueGeneratedOnAdd();
        }
    }
}
