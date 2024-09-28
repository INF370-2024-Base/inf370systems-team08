using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation
{
    public class SchoolEventRepo : ISchoolEventRepo
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository;


        public SchoolEventRepo(EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository;
        }


        public async Task<List<SchoolEvent>> GetAllSchoolEvents()
        {
            return await _context.SchoolEvent.FromSqlRaw("EXEC GetAllSchoolEvents").ToListAsync();
        }

        public async Task<SchoolEvent> GetSchoolEventAsync(Guid eventId)
        {
            IQueryable<SchoolEvent> query = _context.SchoolEvent.Where(c => c.EventId == eventId);
            return await query.FirstOrDefaultAsync();
        }


        // Create a new school event and log the action
        public async Task<SchoolEvent> CreateSchoolEventAsync(CreateSchoolEventViewModel model, Guid userId)
        {
            var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);
            if (employee == null)
            {
                return null;
            }

            var schoolEvent = new SchoolEvent
            {
                EventId = Guid.NewGuid(),
                EmployeeId = model.EmployeeId,
                EventName = model.EventName,
                EventType = model.EventType,
                EventDate = model.EventDate,
                EventTime = model.EventTime,
                EventLocation = model.EventLocation,
                EventDescription = model.EventDescription,
                ContactPerson = model.ContactPerson,
                ContactEmail = model.ContactEmail,
                ContactPhoneNumber = model.ContactPhoneNumber
            };

            _context.SchoolEvent.Add(schoolEvent);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "SchoolEvent",
                AffectedEntityID = schoolEvent.EventId,
                NewValue = JsonConvert.SerializeObject(schoolEvent),  // Log the new school event data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
            await _context.SaveChangesAsync();
            return schoolEvent;
        }

        // Update a school event and log the action
        public async Task UpdateSchoolEventAsync(SchoolEvent updatedSchoolEvent, Guid userId)
        {
            var existingEvent = await _context.SchoolEvent.FindAsync(updatedSchoolEvent.EventId);
            if (existingEvent == null) throw new Exception("School event not found");

            _context.Entry(existingEvent).CurrentValues.SetValues(updatedSchoolEvent);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "SchoolEvent",
                AffectedEntityID = updatedSchoolEvent.EventId,
                OldValue = JsonConvert.SerializeObject(existingEvent),  // Log the old data
                NewValue = JsonConvert.SerializeObject(updatedSchoolEvent),  // Log the new data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }

        // Delete a school event and log the action
        public async Task DeleteSchoolEventAsync(SchoolEvent schoolEvent, Guid userId)
        {
            _context.SchoolEvent.Remove(schoolEvent);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "SchoolEvent",
                AffectedEntityID = schoolEvent.EventId,
                OldValue = JsonConvert.SerializeObject(schoolEvent),  // Log the old data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
