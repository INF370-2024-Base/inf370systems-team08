namespace EduProfileAPI.ViewModels
{
    public class RemedialActivityVM
    {
        public Guid RemActId { get; set; }
        public Guid RemFileId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ActivityContent { get; set; }
    }
}
