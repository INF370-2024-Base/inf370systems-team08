using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Subject[]> GetAllSubjectAsync()
        {
            IQueryable<Subject> query = _context.Subject;
            return await query.ToArrayAsync();
        }
        //get by id
        public async Task<SubjectViewModel> GetSubjectByIdAsync(Guid id)
        {
            // need to change to include the employee and the class -- when we have those cruds donw
            var subject = await _context.Subject.FirstOrDefaultAsync(s => s.SubjectId == id);  
            if (subject == null)
            {
                return null;
            }
            return new SubjectViewModel
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                SubjectDescription = subject.SubjectDescription,
            };
        }
        // Create new subject
        public async Task<SubjectViewModel> CreateSubjectAsync(SubjectViewModel model) //change to CreateSubjectViewModel later 
        {
            {
                // for employee and class we need to update later, Once employee is working then do that class is later
                // var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);
              
                
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

                return new SubjectViewModel // this would also change
                {
                    SubjectId = subject.SubjectId,
                    ClassId = subject.ClassId,
                    EmployeeId = subject.EmployeeId,
                    SubjectName = subject.SubjectName,
                    SubjectDescription = subject.SubjectDescription,
                    SubjectYear = subject.SubjectYear,
                };
            }
        }
        //Update subject
        public async Task<SubjectViewModel> UpdateSubjectAsync(SubjectViewModel model) //change to UpdateSubjectViewModel later
        {
            var subject = await _context.Subject.FirstOrDefaultAsync(s => s.SubjectId == model.SubjectId);
            if (subject == null)
            {
                return null;
            }

            // for employee and class we need to update later, Once employee is working then do that class is later
            // var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);
            subject.ClassId = model.ClassId; //change
            subject.EmployeeId = model.EmployeeId; //change 
            subject.SubjectName = model.SubjectName;
            subject.SubjectDescription = model.SubjectDescription;
            subject.SubjectYear = model.SubjectYear;

            await _context.SaveChangesAsync();

            return new SubjectViewModel //change
            {
                SubjectId = subject.SubjectId,
                ClassId = subject.ClassId,
                EmployeeId = subject.EmployeeId,
                SubjectName = subject.SubjectName,
                SubjectDescription = subject.SubjectDescription,
                SubjectYear = subject.SubjectYear,
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

    }
}
