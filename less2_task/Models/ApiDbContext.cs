using Microsoft.EntityFrameworkCore;

namespace less2_task.Models
{
    public class ApiDbContext: DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ImageFile> Images { get; set; }
        public virtual DbSet<SiteConfig> SiteConfig { get; set; }
    }
}
