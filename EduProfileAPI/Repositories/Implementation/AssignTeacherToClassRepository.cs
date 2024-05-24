using EduProfileAPI.ViewModels;
using EduProfileAPI.Models;
using EduProfileAPI.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using EduProfileAPI.Repositories.Interfaces;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AssignTeacherToClassRepository : IAssignTeacherToClassRepository
    {
        private readonly EduProfileDbContext _context;

        public AssignTeacherToClassRepository(EduProfileDbContext context)
        {
            _context = context;
        }



        public async Task<AssignTeacherToClassViewModel> AssignTeacherToClassAsync(AssignTeacherToClassViewModel model)
        { 
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

            classs.EmployeeId = model.EmployeeId;
            _context.Class.Update(classs);
            await _context.SaveChangesAsync();

            return new AssignTeacherToClassViewModel
            {
                ClassId = classs.ClassId,
                EmployeeId = classs.EmployeeId
            };
        }
            
    }
}
