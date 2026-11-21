using AutoMapper;
using less2_task.Models;
using less2_task.Models.DTO;
using less2_task.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace less2_task.Controllers;

[Tags("Category Management")]
[ApiController]
[Route("api/categories")]
public class CategoryController(
    CategoryService categoryService,
    UploadImageService uploadImageService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPagedCategories([FromQuery] PageRequest<CategoryFilter> pageRequest)
    {
        var result = await categoryService.GetPagedCategoriesAsync(pageRequest);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] Category category)
    {
        var added = await categoryService.AddCategoryAsync(category);
        return Ok(added);
    }

    [HttpPatch("{id:int}/name")]

    public async Task<IActionResult> UpdateName(int id, [FromBody] Category updatedName)
    {
        var updated = await categoryService.UpdateCategoryAsync(id, updatedName.Name);
        if(updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var result = await categoryService.DeleteCategoryAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpPost("{id:int}/image")]
    public async Task<IActionResult> UploadImage(int id, [FromForm] IFormFile file)
    {
        try
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(); 
            }

            var image = await uploadImageService.UploadAsync(file);

            if (category.Image != null)
            {
                await uploadImageService.DeleteAsync(category.Image);
            }
            category.Image = image;

            await categoryService.UpdateAsync(category);

            return Ok(new OperationResult
            {
                Success = true,
                ErrorMessage = null
            });
        }
        catch (Exception ex)
        {

            return BadRequest(new OperationResult
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }
}