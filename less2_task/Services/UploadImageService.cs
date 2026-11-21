using less2_task.Models;

namespace less2_task.Services;

public class UploadImageService(ApiDbContext dbContext, IWebHostEnvironment env)
{
    public async Task<ImageFile> UploadAsync(IFormFile file)
    {
        var originalFileName = Path.GetFileName(file.FileName);

        var fileExtension = Path.GetExtension(originalFileName);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

        var filePath = GetFullPath(uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        var image = new ImageFile
        {
            Filename = uniqueFileName,
            OriginalFilename = originalFileName,
            CreatedAt = DateTime.Now,
        };
        await dbContext.AddAsync(image);
        await dbContext.SaveChangesAsync();
        return image;
    }

    private string GetFullPath(string filename)
    {
        var uploadDir = Path.Combine(env.WebRootPath, "uploads");
        return Path.Combine(uploadDir, filename);
    }

    public async Task DeleteAsync(ImageFile file)
    {
        dbContext.Remove(file);

        var fullPath = GetFullPath(file.Filename);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        await dbContext.SaveChangesAsync();
    }
}
