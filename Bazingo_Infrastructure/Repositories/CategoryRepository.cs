    using Bazingo_Core.Entities;
    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<CategoryRepository> _logger;

            public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger) : base(context)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
            {
                try
                {
                    return await _dbSet
                        .Include(c => c.SubCategories)
                        .Where(c => c.ParentCategoryId == null && !c.IsDeleted)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting main categories");
                    return new List<Category>();
                }
            }

            public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
            {
                try
                {
                    return await _dbSet
                        .Where(c => c.ParentCategoryId == parentId && !c.IsDeleted)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting subcategories for parent ID: {ParentId}", parentId);
                    return new List<Category>();
                }
            }

            public async Task<Category> GetCategoryWithProductsAsync(int id)
            {
                try
                {
                    return await _dbSet
                        .Include(c => c.Products.Where(p => !p.IsDeleted))
                            .ThenInclude(p => p.Seller)
                        .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting category with products for ID: {Id}", id);
                    return null;
                }
            }

            public async Task<Category> GetCategoryWithSubCategoriesAsync(int id)
            {
                try
                {
                    return await _dbSet
                        .Include(c => c.SubCategories.Where(sc => !sc.IsDeleted))
                        .Include(c => c.ParentCategory)
                        .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting category with subcategories for ID: {Id}", id);
                    return null;
                }
            }

            public new async Task<IEnumerable<Category>> GetAllAsync()
            {
                try
                {
                    return await _dbSet
                        .Include(c => c.ParentCategory)
                        .Where(c => !c.IsDeleted)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all categories");
                    return new List<Category>();
                }
            }

            public async Task<bool> UpdateAsync(Category category)
            {
                try
                {
                    await base.UpdateAsync(category);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating category with ID: {Id}", category.Id);
                    return false;
                }
            }

            public async Task<bool> DeleteAsync(int id)
            {
                try
                {
                    var category = await GetByIdAsync(id);
                    if (category == null)
                        return false;

                    category.IsDeleted = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting category with ID: {Id}", id);
                    return false;
                }
            }

            public async Task<int> GetProductCountAsync(int categoryId)
            {
                try
                {
                    return await _context.Products
                        .CountAsync(p => p.CategoryId == categoryId && !p.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting product count for category ID: {CategoryId}", categoryId);
                    return 0;
                }
            }

            public async Task<bool> IsCategoryEmptyAsync(int categoryId)
            {
                try
                {
                    var hasProducts = await _context.Products
                        .AnyAsync(p => p.CategoryId == categoryId && !p.IsDeleted);

                    var hasSubCategories = await _dbSet
                        .AnyAsync(c => c.ParentCategoryId == categoryId && !c.IsDeleted);

                    return !hasProducts && !hasSubCategories;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking if category is empty for ID: {CategoryId}", categoryId);
                    return false;
                }
            }
        }
    }
