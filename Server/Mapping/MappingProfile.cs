using AutoMapper;
using projServer.Entities;
using Shared.DTOs;
using Shared.DTOs.Auth;
using Shared.Enums;

namespace projServer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
       //Users
            CreateMap<RegisterUserDTO, UserEntity>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture ?? Array.Empty<byte>()));

            CreateMap<UserDTO, UserEntity>()
               .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
               .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture ?? Array.Empty<byte>()));
            CreateMap<UserEntity, UserDTO>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture ?? Array.Empty<byte>()));

            //Rooms
            CreateMap<RoomEntity, RoomDTO>().ReverseMap();

            //Devices or Equipment
            CreateMap<DeviceEntity, DeviceDTO>()
         .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomName : "N/A"));


            CreateMap<DeviceDTO, DeviceEntity>()
                .ForMember(dest => dest.Room, opt => opt.Ignore());
            CreateMap<PCUsageEntity, PCUsageDTO>()
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomName : "N/A"))
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                src.User != null ? $"{src.User.FirstName} {src.User.MiddleName} {src.User.LastName}" : "N/A"  ));

            //Device Logs
            CreateMap<DeviceLogEntity, DeviceLogDTO>()
          .ForMember(dest => dest.ActionByName,
              opt => opt.MapFrom(src => src.ActionBy != null
                  ? $"{src.ActionBy.FirstName} {src.ActionBy.LastName}"
                  : "Unknown"));


            // Repair Requests
            CreateMap<RepairRequestEntity, RepairRequestDTO>()
                 .ForMember(dest => dest.DeviceTag, opt => opt.MapFrom(src => src.Device != null ? src.Device.Tag : string.Empty))
                 .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Device != null && src.Device.Room != null ? src.Device.Room.RoomName : string.Empty))
                 .ForMember(dest => dest.ReportedByUserName, opt => opt.MapFrom(src =>
                     src.ReportedByUser != null
                         ? $"{src.ReportedByUser.FirstName} {src.ReportedByUser.LastName}"
                         : "Unknown"));

            CreateMap<RepairRequestDTO, RepairRequestEntity>()
                .ForMember(dest => dest.Device, opt => opt.Ignore());



        }
    }
}
