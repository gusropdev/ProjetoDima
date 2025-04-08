using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };
            
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(category, 201, "Category created successfully.");
        }
        catch
        {
                return new Response<Category?>(null, 500, "Unable to create category.");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
            {
                return new Response<Category?>(null, 404, "Unable to find category.");
            }
            
            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(category, message: "Category updated successfully.");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Unable to update category.");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (category is null)
            {
                return new Response<Category?>(null, 404, "Unable to find category.");
            }
            
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(category, message: "Category deleted successfully.");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Unable to find category.");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            return category is null
                ? new Response<Category?>(null, 404, "Unable to find category.")
                : new Response<Category?>(category, message: "Category found successfully.");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Unable to find category.");
        }
    }
    
    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = context.Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);
            
            var categories = await query
                .Skip((request.PageNumber -1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            
            var count = await query.CountAsync();
            
            return new PagedResponse<List<Category>>(categories, count, request.PageNumber, request.PageSize);
            
        }
        catch
        {
            return new PagedResponse<List<Category>>(null, 500, "Unable to find categories.");
        }
    }
}