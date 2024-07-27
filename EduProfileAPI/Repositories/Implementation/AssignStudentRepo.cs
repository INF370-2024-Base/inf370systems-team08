using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
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
        public async Task<Student[]> GetAllStudentsAsync()
        {
            IQueryable<Student> query = _context.Student;
            return await query.ToArrayAsync();
        }

        public async Task<Class[]> GetAllClassesAsync()
        {
            var query = _context.Class;
            return await query.ToArrayAsync();
        }

        public async Task<Grade[]> GetAllGradesAsync()
        {
            IQueryable<Grade> query = _context.Grade;
            return await query.ToArrayAsync();
        }

        public async Task<Subject[]> GetAllSubjectAsync()
        {
            IQueryable<Subject> query = _context.Subject;
            return await query.ToArrayAsync();
        }

        public async Task AssignStudentToClassAsync(Guid studentId, Guid classId, Guid gradeId)
        {
            var student = await _context.Student.FindAsync(studentId);
            if (student != null)
            {
                student.ClassId = classId;
                student.GradeId = gradeId;
                await _context.SaveChangesAsync();
            }
        }

        //public async Task AssignStudentToSubjectAsync(Guid studentId, Guid subjectId)
        //{
        //    var studentSubject = new StudentSubject
        //    {
        //        StudentId = studentId,
        //        SubjectId = subjectId
        //    };

        //    _context.StudentSubject.Add(studentSubject);
        //    await _context.SaveChangesAsync();
        //}

        public async Task AssignStudentToGradeAsync(Guid studentId, Guid gradeId)
        {
            var student = await _context.Student.FindAsync(studentId);
            if (student != null)
            {
                student.GradeId = gradeId;
                await _context.SaveChangesAsync();
            }
        }

        public Task<Student[]> GettAllStudentsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
