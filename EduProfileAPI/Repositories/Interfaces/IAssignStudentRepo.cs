using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssignStudentRepo
    {
        Task AssignStudentToSubjectAsync(StudentSubjectVM request);
        Task AssignStudentToClassAsync(Guid studentId, Guid classId);
        Task AssignStudentToGradeAsync(Guid studentId, Guid gradeId);

        Task<List<Student>> GetStudentsByClassIdAsync(Guid classId);
        Task<List<Student>> GetStudentsByGradeIdAsync(Guid gradeId);
        Task<List<Student>> GetStudentsBySubjectIdAsync(Guid subjectId);
        Task<List<Student>> GetFilteredStudentsAsync(Guid? classId, Guid? gradeId, Guid? subjectId);
    }
}
