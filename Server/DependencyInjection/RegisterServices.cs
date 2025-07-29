using Microsoft.Extensions.DependencyInjection;
using projServer.Services.Interfaces;
using projServer.Services.Implementations;

namespace projServer.DependencyInjection
{
    public static class RegisterSercvices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDeviceLogService, DeviceLogService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IRepairRequestService, RepairRequestService>();

            return services;
        }
    }
}
