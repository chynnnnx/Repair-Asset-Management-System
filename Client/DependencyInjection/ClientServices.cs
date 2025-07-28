using Microsoft.Extensions.DependencyInjection;
using Client.Services.Interfaces;
using Client.Helpers;
using Client.Services.Implementations;

namespace Client.DependencyInjection
{
    public static class ClientServices
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IPCUsageService, PCUsageService>();
            services.AddScoped<LogoutManager>();
            services.AddScoped<IDeviceLogService, DeviceLogService>();
            services.AddScoped<IRepairRequestService, RepairRequestService>();


            return services;
        }
    }
}
