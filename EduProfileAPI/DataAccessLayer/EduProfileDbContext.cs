using Microsoft.EntityFrameworkCore;
using EduProfileAPI.Models.User;
using EduProfileAPI.Models.Class;
using EduProfileAPI.Models;

namespace EduProfileAPI.DataAccessLayer
{
    public class EduProfileDbContext : DbContext
    {
        public EduProfileDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public  DbSet<Class> Classes { get; set; }
        public DbSet<StudentEducationPhase> StudentEducationPhase { get; set; }
        public DbSet<Grade> Grade { get; set; }
        




    }
}
