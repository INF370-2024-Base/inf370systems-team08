namespace EduProfileAPI.Models
{
    public class AuditTrail
    {
        public Guid AuditTrailId { get; set; } = Guid.NewGuid(); // Automatically generate a GUID
        public Guid UserId { get; set; } // The user who performed the action
        public string Action { get; set; } // The action type (e.g., 'CREATE', 'UPDATE', 'DELETE')
        public string EntityName { get; set; } // The name of the entity being changed (e.g., 'Merit')
        public Guid? AffectedEntityID { get; set; } // The ID of the affected entity (e.g., the Merit ID)
        public string? OldValue { get; set; } // JSON string for old values (if applicable)
        public string? NewValue { get; set; } // JSON string for new values (if applicable)
        public DateTime TimeStamp { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
    }
}
