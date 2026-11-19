using Microsoft.EntityFrameworkCore;
using less2_task.Models;
using less2_task.Models.DTO;

namespace less2_task.Services;

public class CategoryService(ApiDbContext dbContext)
{
    public async Task<PageResult<Category>> GetPagedCategoriesAsync(PageRequest<CategoryFilter> pageRequest)
    {
        Func<IQueryable<Category>, IQueryable<Category>> filter = (q) =>
        {
            if (pageRequest.Filter != null)
            {
                if (!string.IsNullOrEmpty(pageRequest.Filter.Name))
                {
                    q = q.Where(c => c.Name.Contains(pageRequest.Filter.Name));
                }
                if (pageRequest.Filter.CreatedFrom.HasValue)
                {
                    q = q.Where(c => c.CreatedAt >= pageRequest.Filter.CreatedFrom.Value);
                }
                if (pageRequest.Filter.CreatedTo.HasValue)
                {
                    q = q.Where(c => c.CreatedAt <= pageRequest.Filter.CreatedTo.Value);
                }
            }
            return q;
        };

        Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = (q) =>
        {
            return pageRequest.OrderBy switch
            {
                "Id" => q.OrderBy(c => c.Id),
                "-Id" => q.OrderByDescending(c => c.Id),
                "Name" => q.OrderBy(c => c.Name),
                "-Name" => q.OrderByDescending(c => c.Name),
                "CreatedAt" => q.OrderBy(c => c.CreatedAt),
                "-CreatedAt" => q.OrderByDescending(c => c.CreatedAt),
                "UpdatedAt" => q.OrderBy(c => c.UpdatedAt),
                "-UpdatedAt" => q.OrderByDescending(c => c.UpdatedAt),
                _ => q.OrderBy(c => c.Id)
            };
        };

        Func<IQueryable<Category>, IQueryable<Category>> paginate = (q) =>
        {
            return q
            .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
            .Take(pageRequest.PageSize);
        };

        var query = dbContext.Categories.AsQueryable();

        query = filter(query);
        query = orderBy(query);

        var totalItems = await query.CountAsync();

        query = paginate(query);

        var categories = await query.ToListAsync();

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
