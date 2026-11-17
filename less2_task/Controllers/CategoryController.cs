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
public class CategoryController(CategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPagedCategories([FromQuery] PageRequest pageRequest)
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
}
