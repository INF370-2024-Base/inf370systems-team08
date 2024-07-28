using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class SchoolEventRepo : ISchoolEventRepo
    {
        private readonly EduProfileDbContext _context;
        

        public SchoolEventRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<SchoolEvent> CreateSchoolEvent([FromBody]CreateSchoolEventViewModel model)
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
            await _context.SaveChangesAsync();
            return schoolEvent;

        }

        public async Task<SchoolEvent[]> GetAllSchoolEvents()
        {
            return await _context.SchoolEvent.ToArrayAsync();
        }
    }
}
