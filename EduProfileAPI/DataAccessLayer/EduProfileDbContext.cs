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

        public DbSet<IdentityUser> AspNetUsers { get; set; }
        public DbSet<IdentityUserClaim<string>> AspNetUserClaims { get; set; }
        public DbSet<IdentityUserRole<string>> AspNetUserRoles { get; set; }
        public DbSet<IdentityRole> AspNetRoles { get; set; }

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
        public DbSet<EmployeeStatus> EmployeeStatus { get; set; }
        public DbSet<StudentIncident> studentIncident { get; set; }
        public DbSet<MaintenancePriority> MaintenancePriority { get; set; }
        public DbSet<MaintenanceStatus> MaintenanceStatus { get; set; }
        public DbSet<MaintenanceType> MaintenanceType { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequest { get; set; }
        public DbSet<MaintenanceProcedure> MaintenanceProcedure { get; set; }
        public DbSet<StudentAttendance> StudentAttendance { get; set; }
        public DbSet<AttendanceStatus> AttendanceStatus { get; set; }
        public DbSet<AssesmentMark> AssesmentMark { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<ReportType> ReportType { get; set; }
        public DbSet<StudentDocumentType> StudentDocumentType { get; set; }
        public DbSet<MeritType> MeritType { get; set; }
        public DbSet<Assesment> Assesment { get; set; }
        public DbSet<EarlyReleases> EarlyReleases { get; set; }
        public DbSet<SchoolEvent> SchoolEvent { get; set; }
        public DbSet<RemedialFile> RemedialFile { get; set;}
        public DbSet<RemedialActivity> RemedialActivity{ get; set; }
        public DbSet<StudentIncident> StudentIncident { get; set; }
        public DbSet<IncidentType> IncidentType { get; set; }
        public DbSet<StudentSubject> StudentSubject { get; set; }
        public DbSet<DisciplinaryType> DisciplinaryType { get; set; }

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

            modelBuilder.Entity<Student>()
                .HasOne<Parent>()
                .WithMany()
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne<Grade>()
                .WithMany()
                .HasForeignKey(s => s.GradeId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent grade deletion if associated with students

            modelBuilder.Entity<Student>()
                .HasOne<Class>()
                .WithMany()
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent class deletion if associated with students

            modelBuilder.Entity<StudentSubject>()
                .HasKey(ss => ss.StudentSubjectId);  // Now using a single primary key

            // Keep the foreign keys
            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.StudentId);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId);

            modelBuilder.Entity<StudentIncident>()
                .HasOne<Student>()
                .WithMany() // One student many incidents
                .HasForeignKey(si => si.StudentId);

            modelBuilder.Entity<Merit>()
                .HasOne<Student>()
                .WithMany() // One student many Merits
                .HasForeignKey(m => m.StudentId);

            modelBuilder.Entity<Disciplinary>()
                .HasOne<Student>()
                .WithMany() // One Student many Disciplinaries
                .HasForeignKey(d => d.StudentId);

            modelBuilder.Entity<StudentDoc>()
                .HasOne<Student>()
                .WithMany() // One Student many Docs
                .HasForeignKey(sd => sd.StudentId);

            modelBuilder.Entity<MeritType>()
                .HasMany<Merit>() // One MeritType can be associated with many Merits
                .WithOne() // Each Merit has one MeritType
                .HasForeignKey(m => m.MeritTypeId) // Foreign key in Merit to MeritType
                .OnDelete(DeleteBehavior.Restrict); // Prevent MeritType deletion if associated with Merits

            modelBuilder.Entity<MaintenanceType>()
                .HasMany<MaintenanceRequest>() // each MaintenanceType can be associated with many MaintenanceRequests
                .WithOne() //MaintenanceRequest has one MaintenanceType
                .HasForeignKey(mr => mr.MaintenanceTypeId) // Foreign key in MaintenanceRequest pointing to MaintenanceType
                .OnDelete(DeleteBehavior.Restrict); // Prevent MaintenanceType deletion if associated with MaintenanceRequests

            modelBuilder.Entity<IncidentType>()
                 .HasMany<StudentIncident>() // each IncidentType can be associated with many StudentIncidents
                 .WithOne() //tudentIncident has one IncidentType
                 .HasForeignKey(si => si.IncidentTypeId) // Foreign key in StudentIncident pointing to IncidentType
                 .OnDelete(DeleteBehavior.Restrict); // Prevent IncidentType deletion if associated with StudentIncidents

            modelBuilder.Entity<DisciplinaryType>()
                .HasMany<Disciplinary>() // Assuming each DisciplinaryType can be associated with many Disciplinaries
                .WithOne() // Each Disciplinary has one DisciplinaryType
                .HasForeignKey(d => d.DisciplinaryTypeId) // Foreign key in Disciplinary pointing to DisciplinaryType
                .OnDelete(DeleteBehavior.Restrict); // Prevent DisciplinaryType deletion if associated with Disciplinaries

            modelBuilder.Entity<StudentDocumentType>()
                .HasMany<StudentDoc>() // Assuming each StudentDocumentType can be associated with many StudentDocs
                .WithOne() // Each StudentDoc has one StudentDocumentType
                .HasForeignKey(sd => sd.StuDocumentId) // Foreign key in StudentDoc pointing to StudentDocumentType
                .OnDelete(DeleteBehavior.Restrict); // Prevent StudentDocumentType deletion if associated with StudentDocs





            base.OnModelCreating(modelBuilder);
        }
    }
}
