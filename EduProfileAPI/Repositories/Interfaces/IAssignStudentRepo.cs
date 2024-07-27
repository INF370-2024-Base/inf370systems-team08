using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssignStudentRepo
    {
        Task<Class[]> GetAllClassesAsync();
        Task<Student[]> GettAllStudentsAsync();
        Task<Grade[]> GetAllGradesAsync();
        Task<Subject[]> GetAllSubjectAsync();
        //Task AssignStudentToSubjectAsync(Guid studentId, Guid subjectId);
        Task AssignStudentToClassAsync(Guid studentId, Guid classId, Guid gradeId);
        Task AssignStudentToGradeAsync(Guid studentId, Guid gradeId);
    }
}
