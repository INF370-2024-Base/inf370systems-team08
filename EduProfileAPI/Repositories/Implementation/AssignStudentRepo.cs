using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AssignStudentRepo : IAssignStudentRepo
    {
        private readonly EduProfileDbContext _context;

        public AssignStudentRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task AssignStudentToClassAsync(Guid studentId, Guid classId)
        {
            var student = await _context.Student.FindAsync(studentId);
            if (student != null)
            {
                student.ClassId = classId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignStudentToSubjectAsync(StudentSubjectVM request)
        {
            try
            {
                var studentExists = await _context.Student.AnyAsync(s => s.StudentId == request.StudentId);
                var subjectExists = await _context.Subject.AnyAsync(s => s.SubjectId == request.SubjectId);
                var gradeExists = await _context.Grade.AnyAsync(g => g.GradeId == request.GradeId);

                if (!studentExists)
                {
                    throw new ArgumentException("The studentId does not exist.");
                }

                if (!subjectExists)
                {
                    throw new ArgumentException("The subjectId does not exist.");
                }

                var studentSubject = new StudentSubject
                {
                    StudentSubjectId = Guid.NewGuid(),
                    StudentId = request.StudentId,
                    GradeId = request.GradeId,
                    SubjectId = request.SubjectId
                };

                _context.StudentSubject.Add(studentSubject);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }



        public async Task AssignStudentToGradeAsync(Guid studentId, Guid gradeId)
        {
            var student = await _context.Student.FindAsync(studentId);
            if (student != null)
            {
                student.GradeId = gradeId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Student>> GetStudentsByClassIdAsync(Guid classId)
        {
            return await _context.Student
                .Where(s => s.ClassId == classId)
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByGradeIdAsync(Guid gradeId)
        {
            return await _context.Student
                .Where(s => s.GradeId == gradeId)
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsBySubjectIdAsync(Guid subjectId)
        {
            return await _context.StudentSubject
                .Where(ss => ss.SubjectId == subjectId)
                .Select(ss => ss.Student)
                .ToListAsync();
        }
    }
}
