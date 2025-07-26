using projServer.Entities;

namespace projServer.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<UserEntity>
    {
        Task<UserEntity?> GetByEmailAsync(string email);
    
    }
}
