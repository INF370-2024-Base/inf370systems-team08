namespace EduProfileAPI.ViewModels
{
    public class StudentIncidentVM
    {
            public Guid IncidentId { get; set; }
            public Guid StudentId { get; set; }
            public Guid IncidentTypeId { get; set; }
            public DateTime IncidentDate { get; set; }
            public TimeSpan IncidentTime { get; set; }
            public string IncidentLocation { get; set; }
            public string IncidentDescription { get; set; }
            public string ReportedBy { get; set; }
            public DateTime ReportedDate { get; set; }
            public string IncidentStatus { get; set; }
            public bool ParentNotified { get; set; }
            public string Comments { get; set; }
            public string IncidentAttachment { get; set; }
    }
}
