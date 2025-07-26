using System.Linq.Expressions;

namespace projServer.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetValuesAsync();
        Task<T?> FindOneAsync(Expression<Func<T, bool>> filter);
        Task<T?> GetByIdAsync(int id);
        Task<int> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
