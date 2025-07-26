using Microsoft.EntityFrameworkCore;
using projServer.Data;
using projServer.Entities;
using projServer.Repositories.Interfaces;

namespace projServer.Repositories.Implementations
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
    
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext): base (dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

       
    }
}
