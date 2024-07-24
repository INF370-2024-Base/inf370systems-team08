using Microsoft.EntityFrameworkCore;
using EduProfileAPI.Models.User;
using EduProfileAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EduProfileAPI.Models.Maintenance;

namespace EduProfileAPI.DataAccessLayer
{
    public class EduProfileDbContext : IdentityDbContext<IdentityUser>
    {
        public EduProfileDbContext(DbContextOptions<EduProfileDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<StudentEducationPhase> StudentEducationPhase { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Merit> Merit { get; set; }
        public DbSet<Subject> Subject { get; set; } 
        public DbSet<StudentDoc> StudentDocument { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentAnnouncement> StudentAnnouncement { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<MaintenancePriority> MaintenancePriority { get; set; }
        public DbSet<MaintenanceStatus> MaintenanceStatus { get; set; }
        public DbSet<MaintenanceType> MaintenanceType { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequest { get; set; }
        public DbSet<MaintenanceProcedure> MaintenanceProcedure { get; set; }







    }
}
