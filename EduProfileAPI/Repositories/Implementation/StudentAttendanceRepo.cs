using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Runtime.InteropServices;


namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentAttendanceRepo : IStudentAttendanceRepo
    {
        private readonly EduProfileDbContext _context;
        public StudentAttendanceRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentAttendance>> GetStudentAttendanceByClassAndDate(Guid classId, DateTime date)
        {
            return await _context.StudentAttendance
               .Where(sa => sa.ClassId == classId && sa.AttendanceDate == date)
               .ToListAsync();
        }

        public async Task<StudentClassListAttendanceVM[]> GetStudentClassListByClass(Guid classId)
        {
            var students = await _context.Student
                .Where(sc => sc.ClassId == classId)
                .Select(sc => new StudentClassListAttendanceVM {
                    StudentId = sc.StudentId,
                    FirstName = sc.FirstName,
                    LastName = sc.LastName
                })
                .ToArrayAsync();

            return students;
        }

        public async Task<AttendanceStatusViewModel[]> GetAllAttendanceStatus()
        {
            return await _context.AttendanceStatus
                 .Select(
                a => new AttendanceStatusViewModel
                {
                    AttendanceStatusId = a.AttendanceStatusId,
                    StatusDescription = a.StatusDescription
                })
                 .ToArrayAsync();

        }

        public async Task<StudentAttendance> RecordStudentAttendanceAsync(StudentAttendanceViewModel model, Guid userId)
        {
            try
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);
                if (student == null) throw new Exception("Student not found.");

                var classs = await _context.Class.FirstOrDefaultAsync(c => c.ClassId == model.ClassId);
                if (classs == null) throw new Exception("Class not found.");

                var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);
                if (employee == null) throw new Exception("Employee not found.");

                var studentAttendance = new StudentAttendance
                {
                    StudentAttendanceId = Guid.NewGuid(),
                    StudentId = model.StudentId,
                    ClassId = model.ClassId,
                    EmployeeId = model.EmployeeId,
                    AttendanceDate = model.AttendanceDate,
                    AttendanceStatusId = model.AttendanceStatusId,
                };

                await _context.StudentAttendance.AddAsync(studentAttendance);
                await _context.SaveChangesAsync();

                // Log creation to audit trail
                var auditTrail = new AuditTrail
                {
                    UserId = userId,
                    Action = "CREATE",
                    EntityName = "StudentAttendance",
                    AffectedEntityID = studentAttendance.StudentAttendanceId,
                    NewValue = JsonConvert.SerializeObject(studentAttendance),
                    TimeStamp = DateTime.UtcNow
                };
                _context.AuditTrail.Add(auditTrail);
                await _context.SaveChangesAsync();

                return studentAttendance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RecordStudentAttendance: {ex.Message}");
                throw;
            }
        }

        public async Task<StudentAttendance> UpdateStudentAttendance(Guid studentId, UpdateStudentAttendanceVM model, Guid userId)
        {
            var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null) return null;

            var studentAttendance = await _context.StudentAttendance.FirstOrDefaultAsync(sa => sa.StudentAttendanceId == model.StudentAttendanceId);
            if (studentAttendance == null) return null;

            // Capture old value for audit trail
            var oldAttendance = JsonConvert.SerializeObject(studentAttendance);

            studentAttendance.AttendanceStatusId = model.AttendanceStatusId;
            studentAttendance.AttendanceDate = model.AttendanceDate;
            studentAttendance.EmployeeId = model.EmployeeId;
            studentAttendance.ClassId = model.ClassId;
            studentAttendance.StudentId = model.StudentId;

            _context.StudentAttendance.Update(studentAttendance);
            await _context.SaveChangesAsync();

            // Log update to audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "StudentAttendance",
                AffectedEntityID = studentAttendance.StudentAttendanceId,
                OldValue = oldAttendance,
                NewValue = JsonConvert.SerializeObject(studentAttendance),
                TimeStamp = DateTime.UtcNow
            };
            _context.AuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();

            return studentAttendance;
        }

        public async Task<List<StudentAttendance>> GetStudentAttendanceByClassId(Guid classId)
        {
            return await _context.StudentAttendance
                .Where(sa => sa.ClassId == classId)
                .ToListAsync();
        }
    }
}

