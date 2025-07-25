using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projServer.Entities;

namespace projServer.Data.Configurations
{
    public class DeviceLogConfiguration : IEntityTypeConfiguration<DeviceLogEntity>
    {
        public void Configure(EntityTypeBuilder<DeviceLogEntity> builder)
        {
            builder.ToTable("DeviceLogs");

            builder.HasKey(x => x.LogId);

            builder.Property(x => x.Note)
                   .HasMaxLength(500);
            builder.HasOne(x => x.ActionBy)
                   .WithMany()
                   .HasForeignKey(x => x.ActionById)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(x => x.Device)
                   .WithMany() 
                   .HasForeignKey(x => x.DeviceID)
                   .OnDelete(DeleteBehavior.SetNull); 

            builder.Property(x => x.DateLogged)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
