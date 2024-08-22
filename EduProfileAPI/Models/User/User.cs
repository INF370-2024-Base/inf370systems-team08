namespace EduProfileAPI.Models.User
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid HelpId { get; set; }
        public string DisplayName { get; set; }
        public byte[] DisplayImage { get; set; }
        public string AspNetUserId { get; set; }
    }
}
