using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssignStudentRepo
    {
        Task<Class[]> GetAllClassesAsync();
        Task<Student[]> GettAllStudentsAsync();
        Task<Grade[]> GetAllGradesAsync();
        Task<Subject[]> GetAllSubjectAsync();
        Task AddStudentSubjectAsync(Guid studentId, Guid subjectId);
        Task AssignStudentToClassAsync(Guid studentId, Guid classId);
        Task AssignStudentToGradeAsync(Guid studentId, Guid gradeId);
    }
}
