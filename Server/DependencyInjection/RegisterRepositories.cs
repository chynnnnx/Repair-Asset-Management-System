using Microsoft.Extensions.DependencyInjection;
using projServer.Repositories.Interfaces;
using projServer.Repositories.Implementations;
using projServer.Entities;

namespace projServer.DependencyInjection
{
    public static class RegisterRepositories
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IBaseRepository<RoomEntity>, BaseRepository<RoomEntity>>();
            services.AddScoped<IBaseRepository<DeviceLogEntity>, BaseRepository<DeviceLogEntity>>();
            services.AddScoped<IDeviceLogRepository, DeviceLogRepository>();
            services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();


            return services;
        }
    }
}
