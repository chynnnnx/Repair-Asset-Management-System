
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using projServer.Entities;
using projServer.Repositories.Interfaces;
using projServer.Services.Interfaces;

using Shared.DTOs.Auth;
using Shared.Enums;

namespace projServer.Services.Implementations
{
    public class UserService : IUserService
    {


        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, ILogger<UserService> logger, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
        }


        public async Task<UserEntity?> RegisterAsync(RegisterUserDTO userDto)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
                if (existingUser != null) return null;

                var userEntity = _mapper.Map<UserEntity>(userDto);
                userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.PasswordHash);

                userEntity.Role = userDto.Role.ToString();

                await _userRepository.AddAsync(userEntity);
                return userEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register user: {Email}", userDto.Email);
                throw;
            }
        }

        public async Task<LoginResultDTO?> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var userLogin = await _userRepository.GetByEmailAsync(loginDTO.Email);

                if (userLogin == null || !BCrypt.Net.BCrypt.Verify(loginDTO.PasswordHash, userLogin.PasswordHash))
                {
                    return null;
                }

                var token = _tokenService.GenerateJwtToken(userLogin);
                return new LoginResultDTO
                {
                    Token = token,
                    Email = userLogin.Email,
                    Role = userLogin.Role,
                    UserId = userLogin.UserID
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to login user: {Email}", loginDTO.Email);
                throw;
            }
        }


        public async Task <List<UserDTO>>GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetValuesAsync();
                return _mapper.Map<List<UserDTO>>(users);
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all users");
                throw;
            }
        }
        public async Task AddUserAsync(RegisterUserDTO userDTO)
        {
            try
            {
                var userEntity = _mapper.Map<UserEntity>(userDTO);
                userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.PasswordHash);
                await _userRepository.AddAsync(userEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add user: {Email}", userDTO.Email);
                throw;
            }
        }

        public async Task UpdateUserInfo (UserDTO userDTO)
        {
            try
            {
                var userEntity = _mapper.Map<UserEntity>(userDTO);
                await _userRepository.UpdateAsync(userEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user info: {Email}", userDTO.Email);
                throw;
            }
        }

        public async Task DeleteUser(int userId)
        {
            try
            {
                await _userRepository.DeleteAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete user with ID: {UserId}", userId);
                throw;
            }
        }
    }
}
