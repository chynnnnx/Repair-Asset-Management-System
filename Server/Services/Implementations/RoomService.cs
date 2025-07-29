using Shared.DTOs;
using projServer.Repositories.Interfaces;
using projServer.Mapping;
using AutoMapper;
using projServer.Services.Interfaces;
using projServer.Entities;


namespace projServer.Services.Implementations
{
    public class RoomService: IRoomService
    {
        private readonly IBaseRepository<RoomEntity> _roomRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomService> _logger;
        public RoomService(IBaseRepository<RoomEntity> roomRepo, IMapper mapper, ILogger<RoomService> logger)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> AddRoomAsync(RoomDTO roomDto)
        {
            try
            {
                roomDto.RoomName = roomDto.RoomName.Trim();
                var roomEntity = _mapper.Map<RoomEntity>(roomDto);
                await _roomRepo.AddAsync(roomEntity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add room: {RoomName}", roomDto.RoomName);
                return false;
            }
        }


        public async Task<List<RoomDTO>> GetAllRoomsAsync()
        { 
            var roomEntities = await _roomRepo.GetValuesAsync();
            var roomDtos = _mapper.Map<List<RoomDTO>>(roomEntities);
            return roomDtos;    
        }
        public async Task<RoomDTO?> GetRoomByIdAsync(int id)
        {
            try
            {
                var roomEntity = await _roomRepo.GetByIdAsync(id);
                return roomEntity != null ? _mapper.Map<RoomDTO>(roomEntity) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get room by ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> UpdateRoomAsync(RoomDTO roomDto)
        {
            try
            {
                var existing = await _roomRepo.GetByIdAsync(roomDto.RoomId);
                if (existing == null) { return false; }

                var updatedEntity = _mapper.Map<RoomEntity>(roomDto);
                await _roomRepo.UpdateAsync(updatedEntity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update room: {RoomName}", roomDto.RoomName);
                return false;
            }
        }
        
        public async Task<bool> DeleteRoomAsync(int id)
        {
            try
            {
                var existing = await _roomRepo.GetByIdAsync(id);
                if (existing == null) { return false; }

                await _roomRepo.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete room with ID {RoomId}", id);
                return false;
            }
        }

    }
}
