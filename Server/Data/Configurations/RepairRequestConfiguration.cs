using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projServer.Entities;


namespace projServer.Data.Configurations
{
    public class RepairRequestConfiguration: IEntityTypeConfiguration<RepairRequestEntity>
    {
        public void Configure(EntityTypeBuilder<RepairRequestEntity> builder) {

            builder.ToTable("RepairRequests");
            builder.Property(r => r.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.HasOne(r => r.Device)
          .WithMany(d => d.RepairRequests) 
          .HasForeignKey(r => r.DeviceId)
          .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ReportedByUser)
              .WithMany(u => u.RepairRequestsReported)
               .HasForeignKey(x => x.ReportedByUserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
