using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EduProfileAPI.Repositories.Implementation
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly EduProfileDbContext _context;

        public SubjectRepository(EduProfileDbContext context)
        {
            _context = context;
        }
        //get all
        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subject.FromSqlRaw("EXEC GetAllSubjects").ToListAsync();
        }
        //get by id
        public async Task<SubjectViewModel> GetSubjectByIdAsync(Guid id)
        {
            // need to change to include the employee and the class -- when we have those cruds donw
            var subject = await _context.Subject.Include(e => e.Employee).Include(c => c.Class).FirstOrDefaultAsync(s => s.SubjectId == id);
            if (subject == null)
            {
                return null;
            }
            return new SubjectViewModel
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                SubjectDescription = subject.SubjectDescription,
                EmployeeFirstName = subject.Employee.FirstName,
                EmployeeLastName = subject.Employee.LastName,
                ClassName = subject.Class.ClassName,
                SubjectYear = subject.SubjectYear

            };
        }
        // Create new subject
        public async Task<SubjectViewModel> CreateSubjectAsync(CreateSubjectViewModel model) //change to CreateSubjectViewModel later 
        {  //SubjectViewModel is the return type and CreateSubjectViewModel is the input type
                var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);
                if (employee == null)
                {
                    return null;
                }

                var classs = await _context.Class.FirstOrDefaultAsync(c => c.ClassId == model.ClassId);
                if (classs == null)
                {
                    return null;
                }
    
                var subject = new Subject
                {
                    SubjectId = Guid.NewGuid(),
                    ClassId = model.ClassId,
                    EmployeeId = model.EmployeeId,
                    SubjectName = model.SubjectName,
                    SubjectDescription = model.SubjectDescription,
                    SubjectYear = model.SubjectYear,
                };

                await _context.Subject.AddAsync(subject);
                await _context.SaveChangesAsync();

                return new SubjectViewModel 
                {
                    SubjectId = subject.SubjectId,
                    ClassName = classs.ClassName,
                    EmployeeFirstName = employee.FirstName,
                    EmployeeLastName = employee.LastName,
                    SubjectName = subject.SubjectName,
                    SubjectDescription = subject.SubjectDescription,
                    SubjectYear = subject.SubjectYear
                };
        }

        //Update subject
        public async Task<SubjectViewModel> UpdateSubjectAsync(UpdateSubjectViewModel model) //change to UpdateSubjectViewModel later
        {
            var subject = await _context.Subject.FirstOrDefaultAsync(s => s.SubjectId == model.SubjectId);
            if (subject == null)
            {
                return null;
            }

            var classs = await _context.Class.FirstOrDefaultAsync(c => c.ClassId == model.ClassId);
            if (classs == null)
            {
                return null;
            }

            var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);
            if (employee == null)
            {
                return null;
            }
           
            subject.ClassId = model.ClassId; 
            subject.EmployeeId = model.EmployeeId; 
            subject.SubjectName = model.SubjectName;
            subject.SubjectDescription = model.SubjectDescription;
            subject.SubjectYear = model.SubjectYear;

            _context.Subject.Update(subject);
            await _context.SaveChangesAsync();

            return new SubjectViewModel //change
            {
                SubjectId = subject.SubjectId,
                ClassName = classs.ClassName,
                EmployeeFirstName = employee.FirstName,
                EmployeeLastName = employee.LastName,
                SubjectName = subject.SubjectName,
                SubjectDescription = subject.SubjectDescription,
                SubjectYear = subject.SubjectYear
            };
        }
        //delete 
        public async Task<bool> DeleteSubjectAsync(Guid id)
        {
            var subject = await _context.Subject.FirstOrDefaultAsync(s => s.SubjectId == id);
            if (subject == null)
            {
                return false;
            }

            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StudentSubject[]> GetAllStudentSubjectAsync()
        {
            IQueryable<StudentSubject> query = _context.StudentSubject;
            return await query.ToArrayAsync();
        }

    }
}
