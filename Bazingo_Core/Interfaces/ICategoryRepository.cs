using Bazingo_Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> AddAsync(Category category);
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Category>> GetMainCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
        Task<int> GetProductCountAsync(int categoryId);
        Task<bool> IsCategoryEmptyAsync(int categoryId);
    }
}
