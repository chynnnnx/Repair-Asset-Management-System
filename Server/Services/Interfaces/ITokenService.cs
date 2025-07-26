using projServer.Entities;

namespace projServer.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(UserEntity user);
    }
}
