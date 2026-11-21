using less2_task.Services;
using Microsoft.AspNetCore.Mvc;
using less2_task.Models.DTO;

namespace less2_task.Controllers;

[Route("upload")]
[ApiController]
public class UploadController(
    UploadImageService uploadImageService,
    SiteConfigService siteConfigService
    ) : ControllerBase
{
    [HttpPost("background")]
    public async Task<IActionResult> UploadBackgroundImage([FromForm] IFormFile file)
    {
        try
        {
            var image = await uploadImageService.UploadAsync(file);
            await siteConfigService.ChangeBackgroundImageAsync(image);

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

    [HttpGet("background")]
    public async Task<IActionResult> GetBackgroundImage()
    {
        return Ok(new
        {
            ImageUrl = await siteConfigService.GetBackgroundImageUrlAsync()
        });
    }
}
