using Bazingo_Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
        Task<bool> ExistsAsync(int id);
    }
}
