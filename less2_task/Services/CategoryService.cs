using Microsoft.EntityFrameworkCore;
using less2_task.Models;
using less2_task.Models.DTO;

namespace less2_task.Services;

public class CategoryService(ApiDbContext dbContext)
{
    public async Task<PageResult<Category>> GetPagedCategoriesAsync(PageRequest pageRequest)
    {
        var query = dbContext.Categories.AsQueryable();

        var totalItems = await query.CountAsync();

        var categories = await query
            .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
            .Take(pageRequest.PageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageRequest.PageSize);
        var result = new PageResult<Category>
        {
            TotalItems = totalItems,
            TotalPages = totalPages,
            CurrentPage = pageRequest.Page,
            PageSize = pageRequest.PageSize,
            Items = categories
        };
        return result;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await dbContext.Categories.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Category> AddCategoryAsync(Category newCategory)
    {
        newCategory.CreatedAt = DateTime.Now;
        newCategory.UpdatedAt = DateTime.Now;

        await dbContext.Categories.AddAsync(newCategory);
        await dbContext.SaveChangesAsync();
        return newCategory;
    }

    public async Task<Category?> UpdateCategoryAsync(int id, string newName)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(o => o.Id == id);
        if (category == null)
        {
            return null;
        }
        category.Name = newName;
        category.UpdatedAt = DateTime.Now;
        await dbContext.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id);
        if (category == null)
        {
            return false;
        }
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
