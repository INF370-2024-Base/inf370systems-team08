using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentIncidentRepository : IStudentIncidentRepository
    {
        private readonly EduProfileDbContext _context;

        public StudentIncidentRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentIncident>> GetIncidentsAsync()
        {
            return await _context.StudentIncident.FromSqlRaw("EXEC GetIncidents").ToListAsync();
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

        public async Task<IncidentType> GetByTypeIdAsync(Guid? id)
        {
            var studentIncident = await _context.IncidentType.FindAsync(id);

            if (studentIncident == null)
            {
                return null;
            }

            return new IncidentType
            {
 
                IncidentTypeId = studentIncident.IncidentTypeId,
                IncidentSeverity = studentIncident.IncidentSeverity,
                IncidentCategory = studentIncident.IncidentCategory,


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

        public async Task<bool> ExistTypeAsync(Guid id)
        {
            return await _context.IncidentType.AnyAsync(e => e.IncidentTypeId == id);
        }
        public async Task<IEnumerable<IncidentType>> GetAllTypesAsync()
        {
            return await _context.IncidentType
                .Select(si => new IncidentType
                {
                    IncidentTypeId = si.IncidentTypeId,
                    IncidentCategory = si.IncidentCategory,
                    IncidentSeverity = si.IncidentSeverity
                })
                .ToListAsync();
            
        }

  
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<IncidentType>> AddIncidentAsync(IncidentType incidentType)
        {
            incidentType.IncidentTypeId = Guid.NewGuid();
            incidentType.IncidentCategory = incidentType.IncidentCategory;
            incidentType.IncidentSeverity = incidentType.IncidentSeverity;

            await _context.IncidentType.AddAsync(incidentType);
            await _context.SaveChangesAsync();
            return await _context.IncidentType.ToListAsync();
        }


        public async Task<IncidentType> UpdateIncidentType(IncidentType incidentType)
        {
            incidentType.IncidentTypeId = incidentType.IncidentTypeId;
            incidentType.IncidentCategory = incidentType.IncidentCategory ?? string.Empty;
            incidentType.IncidentSeverity = incidentType.IncidentSeverity ?? string.Empty;


            _context.IncidentType.Update(incidentType);
            await _context.SaveChangesAsync();
            return incidentType;
        }

        public async Task<bool> DeleteTypeAsync(Guid id)
        {
            var type = await _context.IncidentType.FindAsync(id);
            if (type == null)
            {
                return false;
            }

            _context.IncidentType.Remove(type);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
