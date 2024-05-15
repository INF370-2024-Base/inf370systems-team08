using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Model
{
    public class EduProfileDbContext: DbContext
    {
        public EduProfileDbContext(DbContextOptions<EduProfileDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
