using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentAttendanceRepo
    {
        Task<IEnumerable<StudentAttendance>> GetStudentAttendanceByClassAndDate(Guid classId, DateTime date);
        Task<StudentClassListAttendanceVM[]> GetStudentClassListByClass(Guid classId);
        Task<AttendanceStatusViewModel[]> GetAllAttendanceStatus();
        Task<StudentAttendance> RecordStudentAttendanceAsync(StudentAttendanceViewModel model);
        Task<StudentAttendance> UpdateStudentAttendance(Guid studentId, UpdateStudentAttendanceVM model);
        Task<StudentAttendance> GetStudentAttendanceById(Guid studentAttendanceId);
    }
}
