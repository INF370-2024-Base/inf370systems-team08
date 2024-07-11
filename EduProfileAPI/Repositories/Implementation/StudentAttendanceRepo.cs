using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Repositories.Interfaces;

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
               .Where(sa => sa.ClassId == classId && sa.AttendanceDate.Date == date.Date)
               .ToListAsync();
        }

    }
}
