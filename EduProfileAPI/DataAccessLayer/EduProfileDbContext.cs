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
        public DbSet<Disciplinary> Disciplinary { get; set; }
        public DbSet<Subject> Subject { get; set; } 
        public DbSet<StudentDoc> StudentDocument { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Parent> Parent { get; set; }
        public DbSet<StudentAnnouncement> StudentAnnouncement { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<MaintenancePriority> MaintenancePriority { get; set; }
        public DbSet<MaintenanceStatus> MaintenanceStatus { get; set; }
        public DbSet<MaintenanceType> MaintenanceType { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequest { get; set; }
        public DbSet<MaintenanceProcedure> MaintenanceProcedure { get; set; }
        public DbSet<StudentAttendance> StudentAttendance { get; set; }
        public DbSet<AttendanceStatus> AttendanceStatus { get; set; }
        public DbSet<AssesmentMark> AssesmentMark { get; set; }
        public DbSet<StudentIncident> StudentIncident { get; set; }
        public DbSet<MeritType> MeritType { get; set; }
        public DbSet<Assesment> Assesment { get; set; }
        public DbSet<RemedialFile> RemedialFile { get; set;}
        public DbSet<RemedialActivity> RemedialActivity{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key
            modelBuilder.Entity<AssesmentMark>()
                .HasKey(am => new { am.StudentId, am.AssesmentId });

            // Configure relationships (optional, if you have navigation properties)
            modelBuilder.Entity<AssesmentMark>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(am => am.StudentId);

            modelBuilder.Entity<AssesmentMark>()
                .HasOne<Assesment>()
                .WithMany()
                .HasForeignKey(am => am.AssesmentId);
        }
    }
}
