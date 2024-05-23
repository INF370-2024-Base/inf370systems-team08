using Microsoft.EntityFrameworkCore;
using EduProfileAPI.Models.User;
using EduProfileAPI.Models;

namespace EduProfileAPI.DataAccessLayer
{
    public class EduProfileDbContext : DbContext
    {
        public EduProfileDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<Grade> Grade { get; set; }
    }
}
