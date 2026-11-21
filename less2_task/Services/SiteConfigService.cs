using less2_task.Models;
using Microsoft.EntityFrameworkCore;

namespace less2_task.Services;

public class SiteConfigService(ApiDbContext dbContext, UploadImageService uploadImageService)
{
    public async Task ChangeBackgroundImageAsync(ImageFile image)
    {
        var siteConfig = await dbContext.SiteConfig
            .Include(x => x.BackgroundImage)
            .FirstOrDefaultAsync();
        if (siteConfig == null)
        {
            siteConfig = new SiteConfig();
            await dbContext.AddAsync(siteConfig);
        }

        if (siteConfig!.BackgroundImage != null)
        {
            await uploadImageService.DeleteAsync(siteConfig.BackgroundImage);
            siteConfig.BackgroundImage = null;
        }

        siteConfig!.BackgroundImage = image;
        await dbContext.SaveChangesAsync();
    }

    public async Task<string?> GetBackgroundImageUrlAsync()
    {
        var siteConfig = await dbContext.SiteConfig
            .Include(x => x.BackgroundImage)
            .FirstOrDefaultAsync();
        if (siteConfig == null)
        {
            siteConfig = new SiteConfig();
            await dbContext.AddAsync(siteConfig);
            await dbContext.SaveChangesAsync();
        }
        var filename = siteConfig!.BackgroundImage?.Filename;

        return !string.IsNullOrWhiteSpace(filename) ? $"/uploads/{filename}" : null;
    }
}
