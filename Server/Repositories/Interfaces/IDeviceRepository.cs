using projServer.Entities;

namespace projServer.Repositories.Interfaces
{
    public interface IDeviceRepository: IBaseRepository<DeviceEntity>
    {

        Task<List<DeviceEntity>> GetAllDeviceWithRoomAsync();
        
    }
}
