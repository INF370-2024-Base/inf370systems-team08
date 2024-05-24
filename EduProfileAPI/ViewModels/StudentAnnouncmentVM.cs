namespace EduProfileAPI.ViewModels
{
    public class StudentAnnouncmentVM
    {
        public Guid ParentId { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public string Description { get; set; }
    }
}
