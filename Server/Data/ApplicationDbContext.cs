using Microsoft.EntityFrameworkCore;
using projServer.Data.Configurations;
using projServer.Entities;
namespace projServer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<RoomEntity> Rooms => Set<RoomEntity>();
        public DbSet<DeviceEntity> Devices => Set<DeviceEntity>();
        public DbSet<DeviceLogEntity> DeviceLogs => Set<DeviceLogEntity>();
        public DbSet<RepairRequestEntity> RepairRequests => Set<RepairRequestEntity>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceLogConfiguration());
            modelBuilder.ApplyConfiguration(new RepairRequestConfiguration());
        }
        }
}
