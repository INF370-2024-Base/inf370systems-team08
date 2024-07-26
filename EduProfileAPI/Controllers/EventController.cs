using Microsoft.AspNetCore.Mvc;
using Google.Apis.Calendar;
using EduProfileAPI.DataAccessLayer;
using Google.Apis.Calendar.v3;
using EduProfileAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Calendar.v3.Data;

namespace EduProfileAPI.Controllers
{
    public class EventController : Controller
    {
        private readonly EduProfileDbContext _context;
        private readonly CalendarService _calendarService;

        public EventController(EduProfileDbContext context, CalendarService calendarService)
        {
            _context = context;
            _calendarService = calendarService;
        }

        // Example: Get All Events
        [HttpGet]
        [Route("GetEvents")]
        public async Task<IActionResult> GetAllEventsAsync()
        {
            var events = await _context.SchoolEvent.ToArrayAsync();
            return Ok(events);
        }

        [HttpPut]
        [Route("Create")]
        public async Task<IActionResult> CreateEvent(SchoolEvent schoolEvent)
        {
            // Add event to database
            _context.SchoolEvent.Add(schoolEvent);
            await _context.SaveChangesAsync();

            // Create Google Calendar event
            var googleEvent = new Event
            {
                Summary = schoolEvent.EventName,
                Location = schoolEvent.EventLocation,
                Description = schoolEvent.EventDescription,
                Start = new EventDateTime { DateTime = schoolEvent.EventDate.Add(schoolEvent.EventTime ?? TimeSpan.Zero) },
                End = new EventDateTime { DateTime = schoolEvent.EventDate.Add(schoolEvent.EventTime ?? TimeSpan.Zero).AddHours(1) } 
            };

            await _calendarService.Events.Insert(googleEvent, "primary").ExecuteAsync();

            return Ok();
        }

        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> UpdateEvent(int eventId, SchoolEvent updatedEvent)
        {
            var eventInDb = await _context.SchoolEvent.FindAsync(eventId);
            if (eventInDb == null)
            {
                return NotFound("Event not found.");
            }

            // Update event details in the database
            eventInDb.EmployeeId = updatedEvent.EmployeeId;
            eventInDb.EventName = updatedEvent.EventName;
            eventInDb.EventType = updatedEvent.EventType;
            eventInDb.EventDate = updatedEvent.EventDate;
            eventInDb.EventTime = updatedEvent.EventTime;
            eventInDb.EventLocation = updatedEvent.EventLocation;
            eventInDb.EventDescription = updatedEvent.EventDescription;
            eventInDb.ContactPerson = updatedEvent.ContactPerson;
            eventInDb.ContactEmail = updatedEvent.ContactEmail;
            eventInDb.ContactPhoneNumber = updatedEvent.ContactPhoneNumber;

            _context.SchoolEvent.Update(eventInDb);
            await _context.SaveChangesAsync();

            // Update the event in Google Calendar
            var googleEvent = new Event
            {
                Summary = eventInDb.EventName,
                Location = eventInDb.EventLocation,
                Description = eventInDb.EventDescription,
                Start = new EventDateTime { DateTime = eventInDb.EventDate.Add(eventInDb.EventTime ?? TimeSpan.Zero) },
                End = new EventDateTime { DateTime = eventInDb.EventDate.Add(eventInDb.EventTime ?? TimeSpan.Zero).AddHours(1) }
            };

            await _calendarService.Events.Update(googleEvent, "primary", eventInDb.GoogleEventId).ExecuteAsync();

            return Ok();
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var eventInDb = await _context.SchoolEvent.FindAsync(eventId);
            if (eventInDb == null)
            {
                return NotFound("Event not found.");
            }

            // Delete the event from the database
            _context.SchoolEvent.Remove(eventInDb);
            await _context.SaveChangesAsync();

            // Delete the event from Google Calendar
            if (!string.IsNullOrEmpty(eventInDb.GoogleEventId))
            {
                await _calendarService.Events.Delete("primary", eventInDb.GoogleEventId).ExecuteAsync();
            }

            return Ok();
        }
    }
}
