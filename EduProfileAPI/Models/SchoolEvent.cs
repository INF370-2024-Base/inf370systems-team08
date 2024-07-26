using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class SchoolEvent
    {
        internal string GoogleEventId;
        [Key]
        public int EventId { get; set; }
        public string EmployeeId { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan? EventTime { get; set; } 
        public string EventLocation { get; set; }
        public string EventDescription { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
    }
}
