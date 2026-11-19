using Microsoft.AspNetCore.Mvc;

namespace less2_task.Controllers;

[Route("upload")]
[ApiController]
public class UploadController(IWebHostEnvironment env): ControllerBase
{
    private static string? currFileName = null;

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        var uploadsFolder = Path.Combine(env.WebRootPath, "uploads");
        if (!string.IsNullOrEmpty(currFileName))
        {
            var oldFilePath = Path.Combine(uploadsFolder, currFileName);
            if(System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
        }

        var originalFileName = Path.GetFileName(file.FileName);

        var fileExtension = Path.GetExtension(originalFileName);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        currFileName = uniqueFileName;

        return Ok(new { FileName = uniqueFileName });
    }
}
