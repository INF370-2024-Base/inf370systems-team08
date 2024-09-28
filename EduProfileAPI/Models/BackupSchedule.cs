namespace EduProfileAPI.Models
{
    public class BackupSchedule
    {
        public string Frequency { get; set; }  // Daily, Weekly, Monthly
        public TimeSpan BackupTime { get; set; }  // Time of day
        public string? DaysOfWeek { get; set; }  // Optional: If weekly, specify days like "Mon,Wed,Fri"
    }
}

