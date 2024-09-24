using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class TeacherClassListRepo : ITeacherClassListRepo
    {
        private readonly EduProfileDbContext _context;
        public TeacherClassListRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Student[]> StudentsInClass(Guid classId)
        { 
            IQueryable<Student> query = _context.Student.Where(s => s.ClassId == classId);
            return await query.ToArrayAsync();
        
        }

        public async Task<Student[]> StudentsForTeachersClass(Guid employeeId)
        {
            var query = from student in _context.Student
                        join cls in _context.Class
                        on student.ClassId equals cls.ClassId
                        where cls.EmployeeId == employeeId
                        select student;

            return await query.ToArrayAsync();
        }
    }
}
