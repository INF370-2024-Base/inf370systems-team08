using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentIncidentRepository : IStudentIncidentRepository
    {
        private readonly EduProfileDbContext _context;

        public StudentIncidentRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentIncident>> GetAllAsync()
        {
            return await _context.studentIncident
                .Select(si => new StudentIncident
                {
                    IncidentId = si.IncidentId,
                    StudentId = si.StudentId,
                    IncidentTypeId = si.IncidentTypeId,
                    IncidentDate = si.IncidentDate,
                    IncidentTime = si.IncidentTime,
                    IncidentLocation = si.IncidentLocation ?? string.Empty,
                    IncidentDescription = si.IncidentDescription ?? string.Empty,
                    ReportedBy = si.ReportedBy ?? string.Empty,
                    ReportedDate = si.ReportedDate,
                    IncidentStatus = si.IncidentStatus ?? string.Empty,
                    ParentNotified = si.ParentNotified,
                    Comments = si.Comments ?? string.Empty,
                    IncidentAttachment = si.IncidentAttachment ?? new byte[0]
                })
                .ToListAsync();
        }

        public async Task<StudentIncident> GetByIdAsync(Guid? id)
        {
            var studentIncident = await _context.studentIncident.FindAsync(id);

            if (studentIncident == null)
            {
                return null;
            }

            return new StudentIncident
            {
                IncidentId = studentIncident.IncidentId,
                StudentId = studentIncident.StudentId,
                IncidentTypeId = studentIncident.IncidentTypeId,
                IncidentDate = studentIncident.IncidentDate,
                IncidentTime = studentIncident.IncidentTime,
                IncidentLocation = studentIncident.IncidentLocation ?? string.Empty,
                IncidentDescription = studentIncident.IncidentDescription ?? string.Empty,
                ReportedBy = studentIncident.ReportedBy ?? string.Empty,
                ReportedDate = studentIncident.ReportedDate,
                IncidentStatus = studentIncident.IncidentStatus ?? string.Empty,
                ParentNotified = studentIncident.ParentNotified,
                Comments = studentIncident.Comments ?? string.Empty,
                IncidentAttachment = studentIncident.IncidentAttachment ?? new byte[0]
            };

        }

        public async Task<StudentIncident> AddAsync(StudentIncident studentIncident)
        {
            studentIncident.IncidentId = Guid.NewGuid(); // Generate a new Guid for the IncidentId
            studentIncident.IncidentTypeId = studentIncident.IncidentTypeId;
            studentIncident.IncidentLocation = studentIncident.IncidentLocation ?? string.Empty;
            studentIncident.IncidentDescription = studentIncident.IncidentDescription ?? string.Empty;
            studentIncident.ReportedBy = studentIncident.ReportedBy ?? string.Empty;
            studentIncident.IncidentStatus = studentIncident.IncidentStatus ?? string.Empty;
            studentIncident.Comments = studentIncident.Comments ?? string.Empty;
            studentIncident.IncidentAttachment = studentIncident.IncidentAttachment ?? new byte[0];

            await _context.studentIncident.AddAsync(studentIncident);
            await _context.SaveChangesAsync();
            return studentIncident;
        }

        public async Task<StudentIncident> UpdateAsync(StudentIncident studentIncident)
        {
            studentIncident.IncidentTypeId = studentIncident.IncidentTypeId;
            studentIncident.IncidentLocation = studentIncident.IncidentLocation ?? string.Empty;
            studentIncident.IncidentDescription = studentIncident.IncidentDescription ?? string.Empty;
            studentIncident.ReportedBy = studentIncident.ReportedBy ?? string.Empty;
            studentIncident.IncidentStatus = studentIncident.IncidentStatus ?? string.Empty;
            studentIncident.Comments = studentIncident.Comments ?? string.Empty;
            studentIncident.IncidentAttachment = studentIncident.IncidentAttachment ?? new byte[0];

            _context.studentIncident.Update(studentIncident);
            await _context.SaveChangesAsync();
            return studentIncident;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var studentIncident = await _context.studentIncident.FindAsync(id);
            if (studentIncident == null)
            {
                return false;
            }

            _context.studentIncident.Remove(studentIncident);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.studentIncident.AnyAsync(e => e.IncidentId == id);
        }
    }
}
